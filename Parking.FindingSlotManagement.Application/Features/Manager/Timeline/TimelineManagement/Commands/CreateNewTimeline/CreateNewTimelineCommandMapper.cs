using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.CreateNewTimeline
{
    public class CreateNewTimelineCommandMapper
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? ExtraFee { get; set; }

        public int? ParkingPriceId { get; set; }
    }
}
