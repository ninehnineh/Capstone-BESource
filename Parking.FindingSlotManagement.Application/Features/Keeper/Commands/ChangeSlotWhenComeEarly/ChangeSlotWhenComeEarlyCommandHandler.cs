using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.ChangeSlotForCustomer;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.ChangeSlotWhenComeEarly
{
    public class ChangeSlotWhenComeEarlyCommandHandler : IRequestHandler<ChangeSlotWhenComeEarlyCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingDetailsRepository _bookingDetailsRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;

        public ChangeSlotWhenComeEarlyCommandHandler(IBookingRepository bookingRepository, IBookingDetailsRepository bookingDetailsRepository, ITimeSlotRepository timeSlotRepository)
        {
            _bookingRepository = bookingRepository;
            _bookingDetailsRepository = bookingDetailsRepository;
            _timeSlotRepository = timeSlotRepository;
        }
        public async Task<ServiceResponse<string>> Handle(ChangeSlotWhenComeEarlyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkInTime = DateTime.UtcNow.AddHours(7);
                var bookingExist = await _bookingRepository.GetById(request.BookingId);
                if (bookingExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy đơn.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                bookingExist.Status = BookingStatus.Check_In.ToString();
                bookingExist.CheckinTime = checkInTime;
                await _bookingRepository.Save();
                List<int> lstTsId = new();
                var oldbookingDetail = await _bookingDetailsRepository.GetParkingSlotIdByBookingDetail(request.BookingId);
                if (oldbookingDetail == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy chi tiết đơn đặt.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                foreach (var item in oldbookingDetail)
                {
                    lstTsId.Add(item.TimeSlot.TimeSlotId);
                }
                var bookingDetailOld = await _bookingDetailsRepository.GetAllItemWithCondition(x => x.BookingId == request.BookingId, null, null, false);
                if (!bookingDetailOld.Any())
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy chi tiết đơn đặt.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var totalHoursEarly = Math.Ceiling((bookingExist.StartTime - checkInTime).TotalHours);
                var x1 = (bookingExist.BookingDetails.FirstOrDefault().TimeSlotId - totalHoursEarly);
                var x2 = bookingExist.BookingDetails.FirstOrDefault().TimeSlotId;
                // Process: delete all booking detail with old timeSlot and add new bookingDetail with new timeSlot
                await _bookingDetailsRepository.DeleteRange(bookingDetailOld.ToList());
               
                if (checkInTime < bookingExist.StartTime)
                {
                    
                    if (totalHoursEarly > 1)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Số tiếng vào sớm chỉ có thể nhỏ hơn 1",
                            StatusCode = 400,
                            Success = false
                        };
                    }
                    var currentDateTime = DateTime.UtcNow.AddHours(7);
                    var checkInTime2 = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, 0, 0);
                    var getListPreviousSlot = await _timeSlotRepository.GetAllItemWithCondition(x => x.StartTime == checkInTime2 && x.ParkingSlotId == request.ParkingSlotId);
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
                            BookingId = bookingExist.BookingId,
                            TimeSlotId = item.TimeSlotId
                        };
                        await _bookingDetailsRepository.Insert(entity);
                        var changeTimeSlotToBooked = await _timeSlotRepository.GetById(item.TimeSlotId);
                        changeTimeSlotToBooked.Status = TimeSlotStatus.Booked.ToString();
                        await _timeSlotRepository.Save();
                    }
                }
                var timeSlotsBooking = await _timeSlotRepository
                   .GetAllTimeSlotsBooking(bookingExist.StartTime, (DateTime)bookingExist.EndTime, request.ParkingSlotId);

                var bookingDetails = new List<BookingDetails>();

                foreach (var timeSlot in timeSlotsBooking)
                {
                    bookingDetails.Add(new BookingDetails { BookingId = bookingExist.BookingId, TimeSlotId = timeSlot.TimeSlotId });
                    timeSlot.Status = TimeSlotStatus.Booked.ToString();
                }

                await _timeSlotRepository.Save();
                await _bookingDetailsRepository.AddRange(bookingDetails);
                //get old time slot and turn into free 
                var lstOldTimeSlot = await _timeSlotRepository.GetAllItemWithCondition(x => lstTsId.Contains(x.TimeSlotId), null, null, false);
                if (!lstOldTimeSlot.Any())
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy time slot cũ.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                foreach (var item in lstOldTimeSlot)
                {
                    if (item.StartTime.Hour >= DateTime.UtcNow.AddHours(7).Hour)
                    {
                        item.Status = TimeSlotStatus.Free.ToString();
                    }
                    await _timeSlotRepository.Save();
                }

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
