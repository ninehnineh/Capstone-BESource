using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetParkingSlots
{
    public class GetParkingSlotsQuery : IRequest<ServiceResponse<IEnumerable<GetParkingSlotsResponse>>>
    {
        public int ParkingId { get; set; }
        //public int FloorId { get; set; }
        //public int TrafficId { get; set; }
        public DateTime StartTimeBooking { get; set; }
        public int DesireHour { get; set; }

    }
}
