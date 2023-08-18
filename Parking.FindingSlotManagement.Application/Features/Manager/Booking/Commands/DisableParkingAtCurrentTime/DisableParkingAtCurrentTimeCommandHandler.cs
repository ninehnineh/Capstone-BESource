using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Newtonsoft.Json;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.DisableParkingAtCurrentTime
{
    public class DisableParkingAtCurrentTimeCommandHandler : IRequestHandler<DisableParkingAtCurrentTimeCommand, ServiceResponse<string>>
    {
        private readonly IParkingSlotRepository parkingSlotRepository;
        private readonly ITimeSlotRepository timeSlotRepository;
        private readonly IBookingDetailsRepository bookingDetailsRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IParkingRepository parkingRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IBookingRepository bookingRepository;

        public DisableParkingAtCurrentTimeCommandHandler(IParkingSlotRepository parkingSlotRepository,
            ITimeSlotRepository timeSlotRepository,
            IBookingDetailsRepository bookingDetailsRepository,
            ITransactionRepository transactionRepository,
            IParkingRepository parkingRepository,
            IWalletRepository walletRepository,
            IBookingRepository bookingRepository)
        {
            this.bookingDetailsRepository = bookingDetailsRepository;
            this.transactionRepository = transactionRepository;
            this.parkingRepository = parkingRepository;
            this.walletRepository = walletRepository;
            this.bookingRepository = bookingRepository;
            this.parkingSlotRepository = parkingSlotRepository;
            this.timeSlotRepository = timeSlotRepository;
        }

        public async Task<ServiceResponse<string>> Handle(DisableParkingAtCurrentTimeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingId = request.ParkingId;
                var disableDate = request.DisableDate;
                var reason = request.Reason;
                var nowUTCDate = DateTime.UtcNow.AddHours(7).Date;
                var disableDateFormated = disableDate.ToString("dd/MM/yyyy");

                ArgumentNullException.ThrowIfNull(parkingId);
                ArgumentNullException.ThrowIfNull(disableDate);

                if (disableDate.AddHours(7).Date == nowUTCDate)
                {
                    var parkingSlots = await parkingSlotRepository.GetParkingSlotsByParkingId(parkingId);
                    var busySlot = await timeSlotRepository.GetBusyParkingSlotId(parkingSlots.ToList());
                    var bookedTimeSlotsAtDisableDate = await timeSlotRepository.GetBookedTimeSlotsByDateTime(parkingSlots.ToList(), disableDate.AddHours(7));
                    if (bookedTimeSlotsAtDisableDate.Count() != 0)
                    {
                        var tempbookingDetails = new List<BookingDetails>();
                        foreach (var item in bookedTimeSlotsAtDisableDate)
                        {

                            var bookingDetails = await bookingDetailsRepository.GetBookingDetailsByTimeSlotId(item.ToList());

                            foreach (var bookingDetail in bookingDetails)
                            {
                                if (!tempbookingDetails.Any(x => x.BookingId == bookingDetail.BookingId))
                                {
                                    tempbookingDetails.Add(bookingDetail);
                                }
                            }

                            var reasonChangeTransactionStatus = reason;

                            var prePaidTransactions = await transactionRepository.GetPrePaidTransactions(tempbookingDetails.ToList());
                            foreach (var prePaidTransaction in prePaidTransactions)
                            {
                                var paidMoney = prePaidTransaction.Price;
                                var customerWallet = prePaidTransaction.Wallet!;
                                var customerWalletId = prePaidTransaction.Wallet!.WalletId;
                                var parkingManagerId = await parkingRepository.GetManagerIdByParkingId(parkingId);
                                var managerWallet = await walletRepository.GetWalletById(parkingManagerId);
                                var managerWalletId = managerWallet.WalletId;
                                var prePaidBookingId = prePaidTransaction.BookingId;

                                customerWallet.Balance += paidMoney;
                                managerWallet.Balance -= paidMoney;

                                Transaction billTrans = new Transaction()
                                {
                                    BookingId = prePaidBookingId,
                                    CreatedDate = DateTime.UtcNow.AddHours(7),
                                    Description = "Hoàn tiền",
                                    Price = paidMoney,
                                    WalletId = customerWalletId,
                                    PaymentMethod = prePaidTransaction.PaymentMethod.ToString(),
                                    Status = prePaidTransaction.Status.ToString()
                                };

                                await transactionRepository.Insert(billTrans);

                                Transaction billTransManager = new Transaction()
                                {
                                    BookingId = prePaidBookingId,
                                    CreatedDate = DateTime.UtcNow.AddHours(7),
                                    Description = "Hoàn tiền cho khách hàng",
                                    Price = paidMoney,
                                    WalletId = managerWalletId,
                                    PaymentMethod = prePaidTransaction.PaymentMethod.ToString(),
                                    Status = prePaidTransaction.Status.ToString()
                                };

                                await transactionRepository.Insert(billTransManager);

                                await walletRepository.Save();
                            }

                            // await transactionRepository.ChangeStatusOriginalTransactionsByBookingDetail(tempbookingDetails.ToList(), reasonChangeTransactionStatus);

                            if (busySlot != 0)
                            {
                                var parkingSlotId = busySlot;
                                var nonBusySlot = parkingSlots.ToList().Where(x => x.ParkingSlotId != parkingSlotId);
                                await timeSlotRepository.DisableTimeSlotByDisableDateTime(nonBusySlot.ToList(), disableDate.AddHours(7));
                            }
                            else
                            {
                                await timeSlotRepository.DisableTimeSlotByDisableDateTime(parkingSlots.ToList(), disableDate);
                            }

                            await bookingRepository.CancelBookedBookingWhenDisableParking(tempbookingDetails.ToList());
                            await parkingRepository.DisableParkingById(parkingId);
                            // Bắn message chưa có token, lỗi, ko for típ dc nên comment
                            // await PushNotiForAllCustomer(tempbookingDetails, reasonChangeTransactionStatus);
                        }
                    }
                    else
                    {
                        // var parkingSlots = await parkingSlotRepository.GetParkingSlotsByParkingId(parkingId)d;
                        if (busySlot != 0)
                        {
                            var parkingSlotId = busySlot;
                            var nonBusySlot = parkingSlots.ToList().Where(x => x.ParkingSlotId != parkingSlotId);
                            await timeSlotRepository.DisableTimeSlotByDisableDateTime(nonBusySlot.ToList(), disableDate.AddHours(7));
                        }
                        else
                        {
                            await timeSlotRepository.DisableTimeSlotByDisableDateTime(parkingSlots.ToList(), disableDate.AddHours(7));
                        }


                        await parkingRepository.DisableParkingById(parkingId);
                    }

                    List<DisableParkingAtCurrentTimeCommandResponse> histories = new List<DisableParkingAtCurrentTimeCommandResponse>();
                    var newhistoryDisableParking = new DisableParkingAtCurrentTimeCommandResponse
                    {
                        ParkingId = parkingId,
                        CreatedAt = DateTime.UtcNow.AddHours(7).ToString("dd/MM/yyyy"),
                        DisableDate = disableDate.ToString("dd/MM/yyyy"),
                        Reason = reason,
                        State = ParkingHistoryStatus.Succeeded.ToString(),
                    };
                    histories.Add(newhistoryDisableParking);
                    string file1 = "historydisableparking.json";
                    if (!File.Exists(file1))
                    {
                        string json = JsonConvert.SerializeObject(histories, Formatting.Indented);
                        File.WriteAllText(file1, json);
                    }
                    else
                    {
                        string jsonFromFile = File.ReadAllText("historydisableparking.json");
                        if (string.IsNullOrWhiteSpace(jsonFromFile))
                        {
                            string json = JsonConvert.SerializeObject(histories, Formatting.Indented);
                            File.WriteAllText(file1, json);
                        }
                        else 
                        {
                            List<DisableParkingAtCurrentTimeCommandResponse> disableParkingHistory = JsonConvert.DeserializeObject<List<DisableParkingAtCurrentTimeCommandResponse>>(jsonFromFile);
                            disableParkingHistory.Add(newhistoryDisableParking);
                            File.WriteAllText("historydisableparking.json", JsonConvert.SerializeObject(disableParkingHistory, Formatting.Indented));
                        }

                    }


                    return new ServiceResponse<string>
                    {
                        Message = "Thành công",
                        StatusCode = 200,
                        Success = true,
                    };
                }

                return new ServiceResponse<string>
                {
                    Message = $"Ngày không hợp lệ, không thể tắt bãi tại {disableDateFormated}",
                    StatusCode = 500,
                    Success = true,
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at DisableParkingAtCurrentTimeCommandHandler: Message {ex.Message}");
            }
        }
    }
}