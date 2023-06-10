using FluentValidation;
using Microsoft.AspNetCore.Connections.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.ApproveBooking
{
    public class ApproveBookingCommandValidation : AbstractValidator<ApproveBookingCommand>
    {
        public ApproveBookingCommandValidation()
        {
            //RuleFor(x => x.BookingId)
            //    .NotNull()
            //    .NotEmpty().WithMessage("");
        }
    }
}
