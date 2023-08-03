using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.AutoCancel.commands.AutomationCancelBooking
{
    public class AutomationCancelBookingCommandHandler : IRequestHandler<AutomationCancelBookingCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;

        public AutomationCancelBookingCommandHandler(IBookingRepository bookingRepository,
            ITimeSlotRepository timeSlotRepository)
        {
            _bookingRepository = bookingRepository;
            _timeSlotRepository = timeSlotRepository;
        }

        public async Task<ServiceResponse<string>> Handle(AutomationCancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository
                .GetBookingIncludeTimeSlot(request.BookingId);

            if (booking == null)
            {
                return new ServiceResponse<string>
                {
                    Message = "Đơn không tồn tại",
                    StatusCode = 200,
                    Success = true
                };
            }

            if (booking.CheckinTime == null)
            {
                booking.Status = BookingStatus.Cancel.ToString();
                await _bookingRepository.Save();
                foreach (var item in booking.BookingDetails)
                {
                    item.TimeSlot.Status = TimeSlotStatus.Free.ToString();
                }
                await _timeSlotRepository.Save();
            }

            return new ServiceResponse<string>
            {
                Message = "Thành công",
                StatusCode = 200,
                Success = true
            };
        }
    }
}
