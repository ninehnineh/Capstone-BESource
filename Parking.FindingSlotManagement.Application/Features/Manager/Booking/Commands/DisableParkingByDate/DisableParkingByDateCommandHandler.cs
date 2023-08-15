using System.Linq.Expressions;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.DisableParkingByDate;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
/// Manager story: manager muốn disable bãi xe tại một ngày đã biết trước (ngày bảo trì bãi, ngày nghỉ lể, ....)
/// Manager nhập ngày muốn nghỉ (disable date), hệ thống check trong disable date có booking nào hay không
/// nếu có thì hủy đơn (refunc nếu là trả trước) hoặc ngược lại và cuối cùng thông báo đến người dùng 
namespace Parking.FindingSlotManagement.Application.Features.Manager.Commands.DisableParkingByDate
{

    public class DisableParkingByDateCommandHandler : IRequestHandler<DisableParkingByDateCommand, ServiceResponse<string>>
    {
        private readonly ITimeSlotRepository timeSlotRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IParkingSlotRepository parkingSlotRepository;
        private readonly IBookingDetailsRepository bookingDetailsRepository;
        private readonly IParkingRepository parkingRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IFireBaseMessageServices fireBaseMessageServices;

        public DisableParkingByDateCommandHandler(ITimeSlotRepository timeSlotRepository,
            ITransactionRepository transactionRepository,
            IParkingSlotRepository parkingSlotRepository,
            IBookingDetailsRepository bookingDetailsRepository,
            IParkingRepository parkingRepository,
            IWalletRepository walletRepository,
            IBookingRepository bookingRepository,
            IFireBaseMessageServices fireBaseMessageServices)
        {
            this.transactionRepository = transactionRepository;
            this.parkingSlotRepository = parkingSlotRepository;
            this.bookingDetailsRepository = bookingDetailsRepository;
            this.parkingRepository = parkingRepository;
            this.walletRepository = walletRepository;
            this.bookingRepository = bookingRepository;
            this.fireBaseMessageServices = fireBaseMessageServices;
            this.timeSlotRepository = timeSlotRepository;
        }

        public async Task<ServiceResponse<string>> Handle(DisableParkingByDateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var nowUTCDate = DateTime.UtcNow.Date;
                var nowDate = DateTime.Now.Date;
                var disableDate = request.DisableDate.Date;
                var disableDateTime = request.DisableDate;
                var disableDateFormated = disableDate.ToString("dd/MM/yyyy");
                var parkingId = request.ParkingId;
                var reason = request.Reason;
                var nextDate = nowUTCDate.AddDays(1);

                ArgumentNullException.ThrowIfNull(parkingId);
                ArgumentNullException.ThrowIfNull(disableDate);
                // nhớ set rule cho disableDate, có cho phép đặt lịch disable 1 tháng hay 1 tuần hay 1 năm 

                if (disableDate >= nowDate.AddDays(30))
                {
                    return new ServiceResponse<string>
                    {
                        Message = $"Chỉ có thể tắt bãi trong vòng một tháng trở lại"
                    };
                }
                if (disableDate == nextDate)
                {
                    return new ServiceResponse<string>
                    {
                        Message = $"Ngày tắt bãi không hợp lệ, không thể tắt bãi tại {disableDateFormated}"
                    };
                }
                if (disableDate <= nowUTCDate)
                {
                    return new ServiceResponse<string>
                    {
                        Message = $"Ngày tắt bãi không hợp lệ, không thể tắt bãi tại {disableDateFormated}"
                    };
                }

                var parkingSlots = await parkingSlotRepository.GetParkingSlotsByParkingId(parkingId);
                var bookedTimeSlotsAtDisableDate = await timeSlotRepository.GetBookedTimeSlotsByDateNew(parkingSlots.ToList(), disableDate);
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
                            // số dư đã biến động, có nên viết transaction ko?
                            await walletRepository.Save();
                        }
                       
                        await transactionRepository.ChangeStatusTransactionsByBookingDetail(tempbookingDetails.ToList(), reasonChangeTransactionStatus);
                        await bookingRepository.CancelBookedBookingWhenDisableParking(tempbookingDetails.ToList());
                        await timeSlotRepository.DisableTimeSlotByDisableDate(parkingSlots.ToList(), disableDate);
                        // Bắn message chưa có token, lỗi, ko for típ dc nên comment
                        // await PushNotiForAllCustomer(tempbookingDetails, reasonChangeTransactionStatus);
                    }
                }
                else
                {
                    // await timeSlotRepository.DisableTimeSlotByDisableDate(parkingId, disableDate); // sai parkingSlotId chuw ko phdai parkingId
                    // var parkingSlots = await parkingSlotRepository.GetParkingSlotsByParkingId(parkingId);
                    // cancel booking
                    await timeSlotRepository.DisableTimeSlotByDisableDate(parkingSlots.ToList(), disableDate);
                }
                var timeToCallJob = disableDateTime - nowUTCDate.AddHours(7);
                BackgroundJob.Schedule<IServiceManagement>(x => x.DisableParkingByDate(parkingId, disableDate), timeToCallJob);

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at DisableParkingSlotByDateCommandHandler: Message {ex.Message}");
            }
        }

        private async Task PushNotiForAllCustomer(List<BookingDetails> tempbookingDetails, string reasonChangeTransactionStatus)
        {
            var customers = await bookingRepository.GetUsersByBookingId(tempbookingDetails.ToList());
            foreach (var customer in customers)
            {
                var token = customer.Devicetoken;
                var pushNotificationMobileModel = new PushNotificationMobileModel
                {
                    TokenMobile = token,
                    Title = "ParkZ thông báo hủy đơn",
                    Message = reasonChangeTransactionStatus,
                };

                await fireBaseMessageServices.SendNotificationToMobileAsync(pushNotificationMobileModel);
            }
        }
    }
}