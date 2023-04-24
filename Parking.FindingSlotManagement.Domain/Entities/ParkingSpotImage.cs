using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class ParkingSpotImage
    {
        public int ParkingSpotImageId { get; set; }
        public string? ImgPath { get; set; }
        public int? ParkingId { get; set; }

        public virtual Parking? Parking { get; set; }
    }
}

