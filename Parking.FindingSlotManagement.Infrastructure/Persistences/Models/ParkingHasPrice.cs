using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class ParkingHasPrice
    {
        public int Id { get; set; }
        public int? ParkingId { get; set; }
        public int? ParkingPriceId { get; set; }

        public virtual Parking? Parking { get; set; }
        public virtual PackagePrice? ParkingPrice { get; set; }
    }
}
