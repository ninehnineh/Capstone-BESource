using MediatR;
using Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetAvailableSlotByFloorId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.GetAvailableSlotByFloorIdForKeeper
{
    public class GetAvailableSlotByFloorIdForKeeperQuery : IRequest<ServiceResponse<IEnumerable<GetAvailableSlotByFloorIdResponse>>>
    {
        public int BookingId { get; set; }
        public int? FloorId { get; set; }
    }
}
