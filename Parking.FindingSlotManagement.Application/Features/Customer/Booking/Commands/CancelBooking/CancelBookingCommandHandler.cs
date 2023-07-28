using Hangfire;
using Hangfire.Common;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System.Linq.Expressions;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CancelBooking
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkingSlotRepository _parkingSlotRepository;
        private readonly IBookingDetailsRepository _bookingDetailsRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IHangfireRepository hangfireRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;

        public CancelBookingCommandHandler(IBookingRepository bookingRepository,
            IParkingSlotRepository parkingSlotRepository,
            IBookingDetailsRepository bookingDetailsRepository,
            ITimeSlotRepository timeSlotRepository,
            IHangfireRepository hangfireRepository
            IWalletRepository walletRepository,
            IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _parkingSlotRepository = parkingSlotRepository;
            _bookingDetailsRepository = bookingDetailsRepository;
            _timeSlotRepository = timeSlotRepository;
            this.hangfireRepository = hangfireRepository;
            _walletRepository = walletRepository;
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<string>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var booking = await _bookingRepository
                    .GetBookingIncludeParkingSlot(request.BookingId);
                List<Expression<Func<BookingDetails, object>>> includes = new()
                {
                    x => x.TimeSlot,
                    x => x.TimeSlot.Parkingslot,
                    x => x.TimeSlot.Parkingslot.Floor.Parking.BusinessProfile
                };

                await hangfireRepository.DeleteJob(request.BookingId);

                var bookingDetail = await _bookingDetailsRepository.GetAllItemWithCondition(x => x.BookingId == booking.BookingId, includes, null, false);
                List<Expression<Func<User, object>>> includesxx = new()
                {
                    x => x.Wallet
                };
                var managerExist = await _userRepository.GetItemWithCondition(x => x.UserId == bookingDetail.FirstOrDefault().TimeSlot.Parkingslot.Floor.Parking.BusinessProfile.UserId, includesxx, false);
                if (booking.Status == BookingStatus.Initial.ToString())
                {
                    foreach (var item in booking.Transactions)
                    {
                        if(item.Status.Equals(TransactionStatus.Da_thanh_toan.ToString()) && item.PaymentMethod.Equals(Domain.Enum.PaymentMethod.tra_truoc.ToString()))
                        {
                            booking.User.Wallet.Balance += item.Price;
                            managerExist.Wallet.Balance -= item.Price;
                            await _walletRepository.Save();
                        }
                    }
                    booking.Status = BookingStatus.Cancel.ToString();
                    await _bookingRepository.Save();
                    foreach (var item in bookingDetail)
                    {
                        item.TimeSlot.Status = TimeSlotStatus.Free.ToString();
                    }
                    await _timeSlotRepository.Save();
                    return new ServiceResponse<string>
                    {
                        Message = "Hủy chỗ đặt thành công",
                        Success = true,
                        StatusCode = 200
                    };
                }

                return new ServiceResponse<string>
                {
                    Message = "Yêu cầu đã được duyệt, không thể hủy chỗ đặt",
                    StatusCode = 400,
                    Success = false,
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
