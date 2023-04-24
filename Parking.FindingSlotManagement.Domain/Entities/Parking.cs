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
        public int? MotoSpot { get; set; }
        public int? CarSpot { get; set; }
        public bool? IsFull { get; set; }
        public bool? IsPrepayment { get; set; }
        public bool? IsOvernight { get; set; }
        public int? Stars { get; set; }
        public int? StarsCount { get; set; }
        public int? ManagerId { get; set; }

        public virtual ICollection<Floor> Floors { get; set; }
        public virtual ICollection<ParkingHasPrice> ParkingHasPrices { get; set; }
        public virtual ICollection<ParkingSlot> ParkingSlots { get; set; }
        public virtual ICollection<ParkingSpotImage> ParkingSpotImages { get; set; }
        public virtual ICollection<StaffParking> StaffParkings { get; set; }
    }
}
