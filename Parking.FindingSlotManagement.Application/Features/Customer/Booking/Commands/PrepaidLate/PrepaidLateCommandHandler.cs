using MediatR;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PrepaidLate
{
    public class PrepaidLateCommandHandler : IRequestHandler<PrepaidLateCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVnPayService _vnPayService;
        private readonly IConfiguration _configuration;
        private readonly IFireBaseMessageServices _fireBaseMessageServices;

        public PrepaidLateCommandHandler(IBookingRepository bookingRepository, IParkingRepository parkingRepository, IBusinessProfileRepository businessProfileRepository, IUserRepository userRepository, IVnPayService vnPayService, IConfiguration configuration, IFireBaseMessageServices fireBaseMessageServices)
        {
            _bookingRepository = bookingRepository;
            _parkingRepository = parkingRepository;
            _businessProfileRepository = businessProfileRepository;
            _userRepository = userRepository;
            _vnPayService = vnPayService;
            _configuration = configuration;
            _fireBaseMessageServices = fireBaseMessageServices;
        }
        public async Task<ServiceResponse<string>> Handle(PrepaidLateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var includes = new List<Expression<Func<Domain.Entities.Booking, object>>>
                //{
                //    //x => x.ParkingSlot,
                //    x => x.User
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
                var manager = await _userRepository
                    .GetItemWithCondition(x => x.ParkingId == parking.ParkingId && x.RoleId == 1);
                if (manager == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Tài khoản manager không tồn tại.",
                        StatusCode = 200,
                        Success = true,
                    };
                }
                var includesUser = new List<Expression<Func<Domain.Entities.BusinessProfile, object>>>
                {
                    x => x.User.VnPays
                };
                var businessAcc = await _businessProfileRepository
                    .GetItemWithCondition(x => x.UserId == manager.UserId, includesUser, true);
                if (businessAcc == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản doanh nghiệp.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if (booking.Status.Equals(BookingStatus.Check_Out.ToString()))
                {
                    if (request.PaymentMethod.Equals(Domain.Enum.PaymentMethod.thanh_toan_tien_mat.ToString()))
                    {
                        booking.Status = BookingStatus.Payment_Successed.ToString();
                        await _bookingRepository.Save();
                        var titleCustomer = _configuration.GetSection("MessageTitle_Customer").GetSection("Payment_Success").Value;
                        var bodyCustomer = _configuration.GetSection("MessageBody_Customer").GetSection("Payment_Success").Value;

                        var pushNotificationMobile = new PushNotificationMobileModel
                        {
                            Title = titleCustomer,
                            Message = bodyCustomer,
                            TokenMobile = booking.User.Devicetoken,
                        };

                        await _fireBaseMessageServices.SendNotificationToMobileAsync(pushNotificationMobile);
                        return new ServiceResponse<string>
                        {
                            Message = "Thành công",
                            Success = true,
                            StatusCode = 201
                        };
                    }
                    else if (request.PaymentMethod.Equals(Domain.Enum.PaymentMethod.thanh_toan_online.ToString()))
                    {
                        BookingTransaction bt = new BookingTransaction
                        {
                            ParkingSlotName = booking.BookingDetails.First().TimeSlot.Parkingslot.Name,
                            //TotalPrice = (decimal)booking.ActualPrice
                        };
                        var url = _vnPayService.CreatePaymentUrl(bt, businessAcc.User.VnPays.FirstOrDefault().TmnCode, businessAcc.User.VnPays.FirstOrDefault().HashSecret, request.context);
                        return new ServiceResponse<string>
                        {
                            Data = url,
                            Message = "Thành công",
                            StatusCode = 201,
                            Success = true
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
