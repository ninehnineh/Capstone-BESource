using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.ChangeSlotForCustomer
{
    public class ChangeSlotForCustomerCommandHandler : IRequestHandler<ChangeSlotForCustomerCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingDetailsRepository _bookingDetailsRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IConflictRequestRepository _conflictRequestRepository;

        public ChangeSlotForCustomerCommandHandler(IBookingRepository bookingRepository, IBookingDetailsRepository bookingDetailsRepository, ITimeSlotRepository timeSlotRepository, IConflictRequestRepository conflictRequestRepository)
        {
            _bookingRepository = bookingRepository;
            _bookingDetailsRepository = bookingDetailsRepository;
            _timeSlotRepository = timeSlotRepository;
            _conflictRequestRepository = conflictRequestRepository;
        }
        public async Task<ServiceResponse<string>> Handle(ChangeSlotForCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
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
                if(!bookingDetailOld.Any())
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy chi tiết đơn đặt.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                // Process: delete all booking detail with old timeSlot and add new bookingDetail with new timeSlot
                await _bookingDetailsRepository.DeleteRange(bookingDetailOld.ToList());
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
                    if(item.StartTime.Hour >= DateTime.UtcNow.AddHours(7).Hour)
                    {
                        item.Status = TimeSlotStatus.Free.ToString();
                    }
                    await _timeSlotRepository.Save();
                }
                var conflictRequest = await _conflictRequestRepository.GetItemWithCondition(x => x.BookingId == request.BookingId, null, false);
                conflictRequest.Status = ConflictRequestStatus.Done.ToString();
                await _conflictRequestRepository.Save();
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
