using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByManagerId
{
    public class GetListParkingByManagerIdResponse
    {
        public int ParkingId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? MotoSpot { get; set; }
        public int? CarSpot { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsFull { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
