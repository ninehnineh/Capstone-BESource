using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Queries.GetListTimelineByParkingPriceId
{
    public class GetListTimelineByParkingPriceIdResponse
    {
        public ParkingPriceRes ParkingPriceRes { get; set; }
        public List<TimeLineRes> LstTimeLineRes { get; set; }
    }
    public class TimeLineRes
    {
        public int TimeLineId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? ExtraFee { get; set; }
    }
    public class ParkingPriceRes
    {
        public int ParkingPriceId { get; set; }
        public string? ParkingPriceName { get; set; }
        public int? StartingTime { get; set; }
        public decimal? PenaltyPrice { get; set; }
        public float? PenaltyPriceStepTime { get; set; }
        public float? ExtraTimeStep { get; set; }

    }
}
