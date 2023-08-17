using Parking.FindingSlotManagement.Application.Models.Traffic;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetParkingDetails;

public class GetParkingDetailsResponse
{
    public ParkingDto Parking { get; set; }
    //public ParkingPriceDto ParkingPrice { get; set; }
    //public ParkingHasPriceDto ParkingHasPrice { get; set; }
}

public class ParkingDto
{
    public int ParkingId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public float? Stars { get; set; }
    public float? TotalStars { get; set; }
    public int? StarsCount { get; set; }
    public int? MotoSpot { get; set; }
    public int? CarSpot { get; set; }
    public bool? IsFull { get; set; }
    public bool? IsPrepayment { get; set; }
    public bool? IsOvernight { get; set; }
    public IEnumerable<ParkingHasPriceDto?> ParkingHasPrices { get; set; }
    public IEnumerable<ParkingSpotImageDto> ParkingSpotImages { get; set; }
}

public class ParkingSpotImageDto
{
    public int ParkingSpotImageId { get; set; }
    public string? ImgPath { get; set; }
}

public class ParkingHasPriceDto
{
    public ParkingPriceDto? ParkingPrice { get; set; }
}

public class ParkingPriceDto
{
    public int ParkingPriceId { get; set; }
    public string? ParkingPriceName { get; set; }
    public int? StartingTime { get; set; }
    public virtual TrafficDto? Traffic { get; set; }
    public float? ExtraTimeStep { get; set; }
    public IEnumerable<TimeLineDto> TimeLines { get; set; }
    public decimal? PenaltyPrice { get; set; }
    public float? PenaltyPriceStepTime { get; set; }

}

public class TimeLineDto
{
    public int TimeLineId { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public bool? IsActive { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public decimal? ExtraFee { get; set; }
}