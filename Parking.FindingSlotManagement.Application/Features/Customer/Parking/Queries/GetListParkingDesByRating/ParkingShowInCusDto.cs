using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetListParkingDesByRating
{
    public class ParkingShowInCusDto
    {
        public int ParkingId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string Avatar { get; set; }
        public float? Stars { get; set; }
    }
}
