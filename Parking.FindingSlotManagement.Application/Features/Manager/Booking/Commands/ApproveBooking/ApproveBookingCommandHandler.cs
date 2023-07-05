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
        private readonly ITimeSlotRepository _timeSlotRepository;

        public ApproveBookingCommandHandler(IBookingRepository bookingRepository,
            IConfiguration configuration,
            IFireBaseMessageServices fireBaseMessageServices, 
            ITimeSlotRepository timeSlotRepository)
        {
            _bookingRepository = bookingRepository;
            _configuration = configuration;
            _fireBaseMessageServices = fireBaseMessageServices;
            _timeSlotRepository = timeSlotRepository;
        }

        public async Task<ServiceResponse<string>> Handle(ApproveBookingCommand request, CancellationToken cancellationToken)
        {
            var bookingId = request.BookingId;

            try
            {

                var booking = await _bookingRepository
                    .GetBooking(bookingId);

                if (booking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Chỗ đặt không tồn tại.",
                        StatusCode = 200,
                        Success = true
                    };
                }

                var bookingSlot = booking.BookingDetails.First().TimeSlot.ParkingSlotId;
                var startTimeOfFirstElement = booking.BookingDetails.First().TimeSlot.StartTime;
                var endTimeOfLastElement = booking.BookingDetails.Last().TimeSlot.EndTime;

                var timeSlotsExist = await _timeSlotRepository
                    .GetAllItemWithCondition(x => x.ParkingSlotId == bookingSlot &&
                        x.StartTime >= startTimeOfFirstElement &&
                        x.EndTime <= endTimeOfLastElement &&
                        x.Status.Equals(TimeSlotStatus.Free.ToString()), null, null, false);

                if (!timeSlotsExist.Any())
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Chỗ đặt hiện tại đang bận, vui lòng chuyển chỗ",
                    };
                }

                booking.Status = BookingStatus.Success.ToString();
                await _bookingRepository.Save();

                timeSlotsExist.ToList().ForEach(x => x.Status = TimeSlotStatus.Booked.ToString());
                await _timeSlotRepository.Save();

                var titleCustomer = _configuration.GetSection("MessageTitle_Customer")
                    .GetSection("Accept").Value;
                var bodyCustomer = _configuration.GetSection("MessageBody_Customer")
                    .GetSection("Accept").Value;
                //var DeviceToken = booking.User.Devicetoken;

                var pushNotificationMobile = new PushNotificationMobileModel
                {
                    Title = titleCustomer,
                    //Message = bodyCustomer + "Vị trí đặt ở " + booking.ParkingSlot.Floor.FloorName + "-" + booking.ParkingSlot.Name,
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
