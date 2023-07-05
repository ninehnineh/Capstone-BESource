using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetParkingById
{
    public class ParkingEntity
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
        public float? Stars { get; set; }
    }
}
