using MediatR;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.BookingInformation;
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
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IBookingDetailsRepository _bookingDetailsRepository;
        private readonly IConflictRequestRepository _conflictRequestRepository;

        public CheckInCommandHandler(IBookingRepository bookingRepository,
            IConfiguration configuration,
            IFireBaseMessageServices fireBaseMessageServices,
            ITimeSlotRepository timeSlotRepository,
            IBookingDetailsRepository bookingDetailsRepository,
            IConflictRequestRepository conflictRequestRepository)
        {
            _bookingRepository = bookingRepository;
            _configuration = configuration;
            _fireBaseMessageServices = fireBaseMessageServices;
            _timeSlotRepository = timeSlotRepository;
            _bookingDetailsRepository = bookingDetailsRepository;
            _conflictRequestRepository = conflictRequestRepository;
        }
        public async Task<ServiceResponse<string>> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            var bookingId = request.BookingId;
            var checkInTime = DateTime.UtcNow.AddHours(7);
            DateTime roundedTime = checkInTime.Date.AddHours(checkInTime.Hour);
            try
            {
                
                /*var include = new List<Expression<Func<Domain.Entities.Booking, object>>>
                {
                    x => x.User,
                    x => x.BookingDetails
                };
*/
                var booking = await _bookingRepository
                    .GetBookingDetailsByBookingIdMethod(bookingId);

                if (booking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Đơn đặt không tồn tại.",
                        StatusCode = 404,
                        Success = false
                    };
                }
                if(!booking.Status.Equals(BookingStatus.Success.ToString()))
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Đơn đang ở trạng thái khác hoặc đã bị hủy nên không thể xử lý.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                var conflictRequest = await _conflictRequestRepository.GetItemWithCondition(x => x.BookingId == bookingId);
                if(conflictRequest != null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Đơn đặt xảy ra lỗi. Do slot đặt có đơn khác lấn giờ.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                else
                {
                    if (checkInTime < booking.StartTime)
                    {
                        var totalHoursEarly = Math.Ceiling((booking.StartTime - checkInTime).TotalHours);
                        if (totalHoursEarly > 1)
                        {
                            return new ServiceResponse<string>
                            {
                                Message = "Số tiếng vào sớm chỉ có thể nhỏ hơn 1",
                                StatusCode = 400,
                                Success = false
                            };
                        }
                        //var getListPreviousSlot = await _timeSlotRepository.GetAllItemWithCondition(x => x.TimeSlotId >= (booking.BookingDetails.FirstOrDefault().TimeSlotId - totalHoursEarly) && x.TimeSlotId < booking.BookingDetails.FirstOrDefault().TimeSlotId);
                        var getListPreviousSlot = await _timeSlotRepository.GetAllItemWithCondition(x => booking.BookingDetails.FirstOrDefault().TimeSlot.ParkingSlotId == x.ParkingSlotId && roundedTime == x.StartTime);
                        var checkBooked = false;
                        foreach (var item in getListPreviousSlot)
                        {
                            if (item.Status.Equals(TimeSlotStatus.Booked.ToString()))
                            {
                                checkBooked = true;
                            }
                        }
                        if (checkBooked)
                        {
                            return new ServiceResponse<string>
                            {
                                Message = "Không thể check-in vào sớm. Tại vì slot vẫn đang có người đặt.",
                                StatusCode = 400,
                                Success = false
                            };
                        }
                        foreach (var item in getListPreviousSlot)
                        {
                            BookingDetails entity = new()
                            {
                                BookingId = booking.BookingId,
                                TimeSlotId = item.TimeSlotId
                            };
                            await _bookingDetailsRepository.Insert(entity);
                            var changeTimeSlotToBooked = await _timeSlotRepository.GetById(item.TimeSlotId);
                            changeTimeSlotToBooked.Status = TimeSlotStatus.Booked.ToString();
                            await _timeSlotRepository.Save();
                        }
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
