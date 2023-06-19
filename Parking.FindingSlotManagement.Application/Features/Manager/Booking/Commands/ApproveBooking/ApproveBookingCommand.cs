using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.ApproveBooking
{
    public class ApproveBookingCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }
        /*public DateTime CheckInTime { get; set; } = DateTime.UtcNow.AddHours(7);*/
    }
}
