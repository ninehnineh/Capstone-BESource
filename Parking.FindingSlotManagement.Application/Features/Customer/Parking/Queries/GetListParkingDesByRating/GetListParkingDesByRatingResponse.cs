using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetListParkingDesByRating
{
    public class GetListParkingDesByRatingResponse
    {
        public ParkingShowInCusDto ParkingShowInCusDto { get; set; }
        public decimal? PriceCar { get; set; }
        public decimal? PriceMoto { get; set; }
    }
}
