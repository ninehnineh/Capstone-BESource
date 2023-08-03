using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CancelBooking
{
    public class CancelBookingCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }

    }
}
