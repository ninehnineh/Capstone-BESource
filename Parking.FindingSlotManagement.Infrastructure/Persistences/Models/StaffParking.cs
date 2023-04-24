using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class StaffParking
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ParkingId { get; set; }

        public virtual Parking? Parking { get; set; }
        public virtual User? User { get; set; }
    }
}
