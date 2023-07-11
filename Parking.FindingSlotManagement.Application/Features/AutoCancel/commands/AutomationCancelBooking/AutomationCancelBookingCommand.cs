using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.AutoCancel.commands.AutomationCancelBooking
{
    public class AutomationCancelBookingCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }

    }
}
