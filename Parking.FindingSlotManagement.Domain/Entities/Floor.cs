using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class Floor
    {
        public int FloorId { get; set; }
        public string? FloorName { get; set; }
        public bool? IsActive { get; set; }
        public int? ParkingId { get; set; }

        public virtual Parking? Parking { get; set; }
        public virtual ICollection<ParkingSlot>? ParkingSlots { get; set; }
    }
}
