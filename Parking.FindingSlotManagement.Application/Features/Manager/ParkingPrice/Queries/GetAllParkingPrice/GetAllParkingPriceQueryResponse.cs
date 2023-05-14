using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Queries.GetAllParkingPrice
{
    public class GetAllParkingPriceQueryResponse
    {
        public int ParkingPriceId { get; set; }

        public string? ParkingPriceName { get; set; }

        public bool? IsActive { get; set; }

    }
}
