using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class Floor
    {
        public Floor()
        {
            ParkingSlots = new HashSet<ParkingSlot>();
        }

        public int Id { get; set; }
        public string? FloorName { get; set; }
        public bool? IsActive { get; set; }
        public int? ParkingId { get; set; }

        public virtual Parking? Parking { get; set; }
        public virtual ICollection<ParkingSlot> ParkingSlots { get; set; }
    }
}
