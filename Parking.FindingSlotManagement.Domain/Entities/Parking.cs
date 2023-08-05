using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class Parking
    {
        public int ParkingId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAvailable { get; set; }
        public int? MotoSpot { get; set; }
        public int? CarSpot { get; set; }
        public bool? IsFull { get; set; }
        public bool? IsPrepayment { get; set; }
        public bool? IsOvernight { get; set; }
        public float? Stars { get; set; }
        public float? TotalStars { get; set; }
        public int? StarsCount { get; set; }
        public int BusinessId { get; set; }

        public BusinessProfile BusinessProfile { get; set; }
        public virtual ICollection<Floor>? Floors { get; set; }
        public virtual ICollection<ParkingHasPrice>? ParkingHasPrices { get; set; }
        public virtual ICollection<ParkingSpotImage>? ParkingSpotImages { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<ApproveParking> ApproveParkings { get; set; }
    }
}
