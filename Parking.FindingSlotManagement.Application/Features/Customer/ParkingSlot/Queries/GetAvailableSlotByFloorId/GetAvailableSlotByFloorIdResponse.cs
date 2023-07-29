using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetAvailableSlotByFloorId
{
    public class GetAvailableSlotByFloorIdResponse
    {
        public ParkingSlotDto ParkingSlotDto { get; set; }
        public int IsBooked { get; set; }
    }
}
