using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CaculateTotalPriceAfterSelectSlot
{
    public class CaculateTotalPriceAfterSelectSlotCommand : IRequest<ServiceResponse<decimal>>
    {
        public int ParkingId { get; set; }

        public DateTime StartimeBooking { get; set; }
        public int DesiredHour { get; set; }
    }
}
