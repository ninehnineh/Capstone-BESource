using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestWithDistance
{
    public class ParkingWithDistanceVer2
    {
        public GetListParkingNearestWithDistanceResponse GetListParkingNearestWithDistanceResponse { get; set; }
        public decimal? PriceCar { get; set; }
        public decimal? PriceMoto { get; set; }
        public double Distance { get; set; }
    }
}
