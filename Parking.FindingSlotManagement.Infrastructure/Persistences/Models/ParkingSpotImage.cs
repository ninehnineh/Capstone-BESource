using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class ParkingSpotImage
    {
        public int Id { get; set; }
        public string? ImgPath { get; set; }
        public int? ParkingId { get; set; }

        public virtual Parking? Parking { get; set; }
    }
}
