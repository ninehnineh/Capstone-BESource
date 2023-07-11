using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PostPaidOnline
{
    public class PostPaidOnlineCommandHandler : IRequestHandler<PostPaidOnlineCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IVnPayService _vnPayService;
        private readonly IParkingRepository _parkingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;

        public PostPaidOnlineCommandHandler(IBookingRepository bookingRepository,
            ITransactionRepository transactionRepository,
            IVnPayService vnPayService, IParkingRepository parkingRepository,
            IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository)
        {
            _bookingRepository = bookingRepository;
            _transactionRepository = transactionRepository;
            _vnPayService = vnPayService;
            _parkingRepository = parkingRepository;
            _userRepository = userRepository;
            _businessProfileRepository = businessProfileRepository;
        }
        public async Task<ServiceResponse<string>> Handle(PostPaidOnlineCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //List<Expression<Func<Domain.Entities.Booking, object>>> includes = new List<Expression<Func<Domain.Entities.Booking, object>>>
                //{
                //    x => x.ParkingSlot
                //};
                var booking = await _bookingRepository
                    .GetBookingIncludeParkingSlot(request.BookingId);

                if (booking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy đơn đặt.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var parking = await _parkingRepository.GetById(request.ParkingId);
                if (parking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var business = await _businessProfileRepository.GetById(parking.BusinessId);
                if (business == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản doanh nghiệp.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var includesUser = new List<Expression<Func<Domain.Entities.User, object>>>
                {
                    x => x.Wallet
                };
                var manager = await _userRepository
                    .GetItemWithCondition(x => x.UserId == business.UserId && x.RoleId == 1, includesUser);
                if (manager == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Tài khoản manager không tồn tại.",
                        StatusCode = 200,
                        Success = true,
                    };
                }
                var lstBookingPaymentExist = await _transactionRepository
                    .GetAllItemWithCondition(x => x.BookingId == request.BookingId, null, null, false);

                var firstBookingPaymentExist = lstBookingPaymentExist.FirstOrDefault();

                if (firstBookingPaymentExist.PaymentMethod.Equals(Domain.Enum.PaymentMethod.tra_sau.ToString()))
                {
                    var userBooking = await _userRepository
                        .GetItemWithCondition(x => x.UserId == booking.UserId, includesUser, false);
                    //tiền trong ví đủ để thanh toán
                    if (userBooking.Wallet.Balance >= booking.TotalPrice && userBooking.Wallet.Debt == 0)
                    {
                        userBooking.Wallet.Balance -= (decimal)booking.TotalPrice;
                        manager.Wallet.Balance += (decimal)booking.TotalPrice;
                        foreach (var item in lstBookingPaymentExist)
                        {
                            item.Status = Domain.Enum.BookingPaymentStatus.Da_thanh_toan.ToString();
                            item.PaymentMethod = request.PaymentMethod;
                            item.WalletId = userBooking.Wallet.WalletId;
                            await _transactionRepository.Save();
                        }
                        booking.Status = Domain.Enum.BookingStatus.Payment_Successed.ToString();
                        await _bookingRepository.Save();
                        await _userRepository.Save();
                        await _userRepository.Update(manager);
                        return new ServiceResponse<string>
                        {
                            Message = "Thành công",
                            StatusCode = 201,
                            Success = true
                        };
                    }
                    //số dư trong ví không đủ để thanh toán
                    else if (userBooking.Wallet.Balance < booking.TotalPrice)
                    {
                        foreach (var item in lstBookingPaymentExist)
                        {
                            item.Status = Domain.Enum.BookingPaymentStatus.Chua_thanh_toan.ToString();
                            await _transactionRepository.Save();
                        }
                        booking.Status = Domain.Enum.BookingStatus.Waiting_For_Payment.ToString();
                        await _bookingRepository.Save();
                        return new ServiceResponse<string>
                        {
                            Message = "Số dư trong ví không đủ để thanh toán.",
                            StatusCode = 400,
                            Success = false
                        };
                    }
                }
                return new ServiceResponse<string>
                {
                    Message = "Trạng thái của đơn không hợp lệ để thực hiện chức năng.",
                    StatusCode = 400,
                    Success = false
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
