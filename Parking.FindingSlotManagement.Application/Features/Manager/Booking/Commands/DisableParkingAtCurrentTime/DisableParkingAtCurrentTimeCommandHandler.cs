using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;

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

                if (disableDate.Date == nowUTCDate)
                {
                    var parkingSlots = await parkingSlotRepository.GetParkingSlotsByParkingId(parkingId);

                    var bookedTimeSlotsAtDisableDate = await timeSlotRepository.GetBookedTimeSlotsByDateTime(parkingSlots.ToList(), disableDate);
                    if (bookedTimeSlotsAtDisableDate != null)
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
                                var parkingManagerId = await parkingRepository.GetManagerIdByParkingId(parkingId);
                                var managerWallet = await walletRepository.GetWalletById(parkingManagerId);

                                customerWallet.Balance += paidMoney;
                                managerWallet.Balance -= paidMoney;

                                Transaction billTrans = new Transaction()
                                {
                                    BookingId = prePaidTransaction.BookingId,
                                    CreatedDate = DateTime.UtcNow.AddHours(7),
                                    Description = "Hoàn tiền",
                                    Price = paidMoney,
                                    WalletId = customerWallet.WalletId,
                                    PaymentMethod = prePaidTransaction.PaymentMethod.ToString(),
                                    Status = prePaidTransaction.Status.ToString()
                                };


                                Transaction billTransManager = new Transaction()
                                {
                                    BookingId = prePaidTransaction.BookingId,
                                    CreatedDate = DateTime.UtcNow.AddHours(7),
                                    Description = "Hoàn tiền cho khách hàng",
                                    Price = paidMoney,
                                    WalletId = managerWallet.WalletId,
                                    PaymentMethod = prePaidTransaction.PaymentMethod.ToString(),
                                    Status = prePaidTransaction.Status.ToString()
                                };

                                await transactionRepository.Insert(billTrans);
                                await transactionRepository.Insert(billTransManager);

                                await walletRepository.Save();
                            }

                            // await transactionRepository.ChangeStatusOriginalTransactionsByBookingDetail(tempbookingDetails.ToList(), reasonChangeTransactionStatus);
                            await bookingRepository.CancelBookedBookingWhenDisableParking(tempbookingDetails.ToList());
                            await timeSlotRepository.DisableTimeSlotByDisableDateTime(parkingSlots.ToList(), disableDate);
                            // Bắn message chưa có token, lỗi, ko for típ dc nên comment
                            // await PushNotiForAllCustomer(tempbookingDetails, reasonChangeTransactionStatus);
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
                    Message = $"Ngày tắt bãi không hợp lệ, không thể tắt bãi tại {disableDateFormated}",
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