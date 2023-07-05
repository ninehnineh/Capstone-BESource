using MediatR;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.ChangeStatusToAlreadyPaid
{
    public class ChangeStatusToAlreadyPaidCommandHandler : IRequestHandler<ChangeStatusToAlreadyPaidCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IConfiguration _configuration;
        private readonly IFireBaseMessageServices _fireBaseMessageServices;
        private readonly IUserRepository _userRepository;

        public ChangeStatusToAlreadyPaidCommandHandler(IBookingRepository bookingRepository, IParkingRepository parkingRepository, IConfiguration configuration, IFireBaseMessageServices fireBaseMessageServices, IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _parkingRepository = parkingRepository;
            _configuration = configuration;
            _fireBaseMessageServices = fireBaseMessageServices;
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<string>> Handle(ChangeStatusToAlreadyPaidCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingRepository.GetItemWithCondition(x => x.BookingId == request.BookingId, null, false);
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
                booking.Status = BookingStatus.Payment_Successed.ToString();
                await _bookingRepository.Save();
                var titleManager = _configuration.GetSection("MessageTitle_Manager").GetSection("Payment_Success").Value;
                var bodyManager = _configuration.GetSection("MessageBody_Manager").GetSection("Payment_Success").Value;

                /*var includeUser = new List<Expression<Func<StaffParking, object>>>
                {
                    x => x.User!,
                };

                var deviceToken = "";
                var staffParking = await _staffParkingRepository
                    .GetAllItemWithCondition(x => x.ParkingId == request.ParkingId, includeUser);

                if (!staffParking.Any())
                {
                    var manager = await _userRepository.GetById(parking.ManagerId);
                    var pushNotificationModel = new PushNotificationWebModel
                    {
                        Title = titleManager,
                        Message = bodyManager + booking.ActualPrice,
                        TokenWeb = manager.Devicetoken,
                    };
                    await _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
                }
                else
                {
                    foreach (var item in staffParking)
                    {
                        deviceToken = item.User.Devicetoken!.ToString();
                        var pushNotificationModel = new PushNotificationWebModel
                        {
                            Title = titleManager,
                            Message = bodyManager + booking.ActualPrice,
                            TokenWeb = deviceToken,
                        };
                        await _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
                    }
                }*/
                var deviceToken = "";
                var managerAccount = await _userRepository.GetAllItemWithCondition(x => x.ParkingId == parking.ParkingId);
                var lstStaff = managerAccount.Where(x => x.RoleId == 2);
                var ManagerOfParking = managerAccount.FirstOrDefault(x => x.RoleId == 1);
                if (lstStaff.Any())
                {
                    foreach (var item in lstStaff)
                    {
                        deviceToken = item.Devicetoken.ToString();
                        var pushNotificationModel = new PushNotificationWebModel
                        {
                            Title = titleManager,
                            //Message = bodyManager + booking.ActualPrice,
                            TokenWeb = deviceToken,
                        };
                        await _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
                    }
                }
                else
                {
                    var manager = await _userRepository.GetById(ManagerOfParking.UserId!);
                    var pushNotificationModel = new PushNotificationWebModel
                    {
                        Title = titleManager,
                        //Message = bodyManager + booking.ActualPrice,
                        TokenWeb = manager.Devicetoken,
                    };
                    await _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
                }
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
                    Success = true
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
