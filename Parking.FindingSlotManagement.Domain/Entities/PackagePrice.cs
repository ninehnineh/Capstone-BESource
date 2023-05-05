using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class PackagePrice
    {
        public int PackagePriceId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
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
