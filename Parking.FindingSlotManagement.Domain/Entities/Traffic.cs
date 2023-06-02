using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class Traffic
    {
        public int TrafficId { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<ParkingPrice>? ParkingPrices { get; set; }
        public virtual ICollection<ParkingSlot>? ParkingSlots { get; set; }
        public virtual ICollection<VehicleInfor>? VehicleInfors { get; set; }
    }
}
