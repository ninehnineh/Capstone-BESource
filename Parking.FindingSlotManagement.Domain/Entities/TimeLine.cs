using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class TimeLine
    {
        public int TimeLineId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? ExtraFee { get; set; }

        public int? ParkingPriceId { get; set; }
        public ParkingPrice? ParkingPrice { get; set; }

    }
}
