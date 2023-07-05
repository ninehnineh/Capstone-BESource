using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class ParkingPrice
    {
        public int ParkingPriceId { get; set; }
        public string? ParkingPriceName { get; set; }
        public bool? IsActive { get; set; }
        public bool IsWholeDay { get; set; }
        public int? StartingTime { get; set; }
        public bool? HasPenaltyPrice { get; set; }
        public decimal? PenaltyPrice { get; set; }
        public float? PenaltyPriceStepTime { get; set; }
        public bool? IsExtrafee { get; set; }
        public float? ExtraTimeStep { get; set; }


        public int? BusinessId { get; set; }
        public BusinessProfile? BusinessProfile { get; set; }   
        public int? TrafficId { get; set; }
        public virtual Traffic? Traffic { get; set; }
        public virtual ICollection<TimeLine>? TimeLines { get; set; }
        public virtual ICollection<ParkingHasPrice>? ParkingHasPrices { get; set; }
    }
}
