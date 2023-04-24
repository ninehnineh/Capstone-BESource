using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class PackagePrice
    {
        public PackagePrice()
        {
            ParkingHasPrices = new HashSet<ParkingHasPrice>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsExtrafee { get; set; }
        public decimal? ExtraFee { get; set; }
        public float? ExtraTimeStep { get; set; }
        public bool? HasPenaltyPrice { get; set; }
        public decimal? PenaltyPrice { get; set; }
        public float? PenaltyPriceStepTime { get; set; }
        public int? TrafficId { get; set; }

        public virtual Traffic? Traffic { get; set; }
        public virtual ICollection<ParkingHasPrice> ParkingHasPrices { get; set; }
    }
}
