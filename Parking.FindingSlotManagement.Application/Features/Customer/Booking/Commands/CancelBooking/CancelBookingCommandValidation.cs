using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CancelBooking
{
    public class CancelBookingCommandValidation : AbstractValidator<CancelBookingCommand>
    {
        public CancelBookingCommandValidation()
        {
            RuleFor(x => x.BookingId)
                .NotNull()
                .NotEmpty();
        }
    }
}
