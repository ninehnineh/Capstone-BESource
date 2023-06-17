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

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.ApproveBooking
{
    public class ApproveBookingCommandHandler : IRequestHandler<ApproveBookingCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IConfiguration _configuration;
        private readonly IFireBaseMessageServices _fireBaseMessageServices;

        public ApproveBookingCommandHandler(IBookingRepository bookingRepository,
            IConfiguration configuration,
            IFireBaseMessageServices fireBaseMessageServices)
        {
            _bookingRepository = bookingRepository;
            _configuration = configuration;
            _fireBaseMessageServices = fireBaseMessageServices;
        }

        public async Task<ServiceResponse<string>> Handle(ApproveBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var include = new List<Expression<Func<Domain.Entities.Booking, object>>>
                {
                    x => x.ParkingSlot,
                    x => x.User,
                    x => x.ParkingSlot,
                    x => x.ParkingSlot.Floor
                };

                var booking = await _bookingRepository
                    .GetItemWithCondition(x => x.BookingId == request.BookingId, include, false);

                if (booking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Chỗ đặt không tồn tại.",
                        StatusCode = 200,
                        Success = true
                    };
                }

                var listBooking = await _bookingRepository
                    .GetAllItemWithCondition(x => x.ParkingSlotId == booking.ParkingSlotId &&
                    x.DateBook.Date == DateTime.UtcNow.AddHours(7).Date);

                var previousBookedSlot = listBooking
                    .Where(x => x.EndTime <= booking.StartTime).FirstOrDefault();

                if (previousBookedSlot == null)
                {
                    //booking.CheckinTime = request.CheckInTime;

                    booking.Status = BookingStatus.Success.ToString();

                    await _bookingRepository.Save();

                    var titleCustomer = _configuration.GetSection("MessageTitle_Customer")
                        .GetSection("Accept").Value;
                    var bodyCustomer = _configuration.GetSection("MessageBody_Customer")
                        .GetSection("Accept").Value;
                    //var DeviceToken = booking.User.Devicetoken;

                    var pushNotificationMobile = new PushNotificationMobileModel
                    {
                        Title = titleCustomer,
                        Message = bodyCustomer + "Vị trí đặt ở " + booking.ParkingSlot.Floor.FloorName + "-" + booking.ParkingSlot.Name,
                        //TokenMobile = DeviceToken,
                    };

                    await _fireBaseMessageServices.SendNotificationToMobileAsync(pushNotificationMobile);

                    return new ServiceResponse<string>
                    {
                        Message = "Thành công",
                        StatusCode = 201,
                        Success = true,
                    };
                }
                else if (previousBookedSlot.CheckoutTime != null &&
                    previousBookedSlot.EndTime < DateTime.UtcNow.AddHours(7))
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Chỗ đặt hiện tại đang bận, vui lòng chuyển chỗ",
                    };
                }

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 201,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
