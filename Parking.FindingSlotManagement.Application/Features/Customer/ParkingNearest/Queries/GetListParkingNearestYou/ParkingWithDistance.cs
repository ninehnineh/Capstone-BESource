using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestYou
{
    public class ParkingWithDistance
    {
        public GetListParkingNearestYouQueryResponse GetListParkingNearestYouQueryResponse { get; set; }
        public decimal? PriceCar { get; set; }
        public double Distance { get; set; }
    }
}
