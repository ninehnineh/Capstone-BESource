using Hangfire;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using RestSharp;
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
        private readonly ILogger<ApproveBookingCommandHandler> _logger;
        private readonly TimeZoneInfo _timeZoneInfo;

        public ApproveBookingCommandHandler(IBookingRepository bookingRepository,
            IConfiguration configuration,
            IFireBaseMessageServices fireBaseMessageServices, 
            ITimeSlotRepository timeSlotRepository,
            ILogger<ApproveBookingCommandHandler> logger)
        {
            _bookingRepository = bookingRepository;
            _configuration = configuration;
            _fireBaseMessageServices = fireBaseMessageServices;
            _timeSlotRepository = timeSlotRepository;
            _logger = logger;
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); ;
        }

        public async Task<ServiceResponse<string>> Handle(ApproveBookingCommand request, CancellationToken cancellationToken)
        {
            var bookingId = request.BookingId;
            try
            {

                var booking = await _bookingRepository
                    .GetBookingIncludeTransaction(bookingId);

                if (booking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Chỗ đặt không tồn tại.",
                        StatusCode = 200,
                        Success = true
                    };
                }

                booking.Status = BookingStatus.Success.ToString();
                await _bookingRepository.Save();

                var isPostPaid = booking.Transactions.First().Status.Equals(BookingPaymentStatus.Chua_thanh_toan.ToString()) &&
                                    booking.Transactions.First().PaymentMethod.Equals(PaymentMethod.tra_sau.ToString());

                var isPrePaid = booking.Transactions.First().Status.Equals(BookingPaymentStatus.Da_thanh_toan.ToString()) &&
                                    booking.Transactions.First().PaymentMethod.Equals(PaymentMethod.tra_truoc.ToString());

                if (isPostPaid)
                {
                    var timeToCancel = booking.StartTime.AddHours(1) - DateTime.UtcNow.AddHours(7);
                    BackgroundJob.Schedule<IServiceManagement>(x => x.AutoCancelBookingWhenOverAllowTimeBooking(bookingId), timeToCancel);
                }
                else if (isPrePaid)
                {
                    var timeToCancel = booking.EndTime.Value - DateTime.UtcNow.AddHours(7);
                    // BackgroundJob.Schedule<IServiceManagement>(x => x.AutoCancelBookingWhenOutOfEndTimeBooking(bookingId), timeToCancel);
                }



                var titleCustomer = _configuration.GetSection("MessageTitle_Customer")
                    .GetSection("Accept").Value;
                var bodyCustomer = _configuration.GetSection("MessageBody_Customer")
                    .GetSection("Accept").Value;
                var DeviceToken = booking.User.Devicetoken;

                var pushNotificationMobile = new PushNotificationMobileModel
                {
                    Title = titleCustomer,
                    Message = bodyCustomer,
                    TokenMobile = DeviceToken,
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
