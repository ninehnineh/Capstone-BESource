using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetAvailableSlotByFloorId
{
    public class GetAvailableSlotByFloorIdQuery : IRequest<ServiceResponse<IEnumerable<GetAvailableSlotByFloorIdResponse>>>
    {
        public int FloorId { get; set; }
        public DateTime StartTimeBooking { get; set; }
        public DateTime EndTimeBooking { get; set; }
    }
}
