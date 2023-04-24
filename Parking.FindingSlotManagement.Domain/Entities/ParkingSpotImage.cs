using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class ParkingSpotImage
    {
        public int Id { get; set; }
        public string? ImgPath { get; set; }
        public int? ParkingId { get; set; }

        public virtual Parking? Parking { get; set; }
    }
}
