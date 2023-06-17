using MediatR;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckIn
{
    public class CheckInCommandHandler : IRequestHandler<CheckInCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IConfiguration _configuration;
        private readonly IFireBaseMessageServices _fireBaseMessageServices;

        public CheckInCommandHandler(IBookingRepository bookingRepository,
            IConfiguration configuration,
            IFireBaseMessageServices fireBaseMessageServices)
        {
            _bookingRepository = bookingRepository;
            _configuration = configuration;
            _fireBaseMessageServices = fireBaseMessageServices;
        }
        public async Task<ServiceResponse<string>> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            var bookingId = request.BookingId;
            var checkInTime = DateTime.UtcNow.AddHours(7);
            try
            {
                var include = new List<Expression<Func<Domain.Entities.Booking, object>>>
                {
                    x => x.User,
                };

                var booking = await _bookingRepository
                    .GetItemWithCondition(x => x.BookingId == bookingId, include, false);

                if (booking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Chỗ đặt không tồn tại.",
                        StatusCode = 200,
                        Success = true
                    };
                }

                if (booking.StartTime > booking.EndTime)
                    booking.EndTime += TimeSpan.FromHours(24);

                //if (booking.EndTime.Value.Date == checkInTime.Date)
                //    checkInTime += TimeSpan.FromHours(24);

                //if (checkInTime >= booking.StartTime && checkInTime < booking.EndTime)
                //{
                booking.Status = BookingStatus.Check_In.ToString();
                //if (checkInTime.Hour > 24)
                //{
                //    checkInTime -= TimeSpan.FromHours(24);
                //}
                booking.CheckinTime = checkInTime;
                //}

                await _bookingRepository.Save();

                var titleCustomer = _configuration.GetSection("MessageTitle_Customer")
                    .GetSection("Checkin").Value;
                var bodyCustomer = _configuration.GetSection("MessageBody_Customer")
                    .GetSection("Checkin").Value;
                var userDiviceToken = booking.User!.Devicetoken;

                var pushNotificationMobile = new PushNotificationMobileModel
                {
                    Title = titleCustomer,
                    Message = bodyCustomer,
                    TokenMobile = userDiviceToken,
                };

                await _fireBaseMessageServices
                    .SendNotificationToMobileAsync(pushNotificationMobile);

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
