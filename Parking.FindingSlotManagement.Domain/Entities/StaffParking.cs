using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class StaffParking
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ParkingId { get; set; }

        public virtual Parking? Parking { get; set; }
        public virtual User? User { get; set; }
    }
}
