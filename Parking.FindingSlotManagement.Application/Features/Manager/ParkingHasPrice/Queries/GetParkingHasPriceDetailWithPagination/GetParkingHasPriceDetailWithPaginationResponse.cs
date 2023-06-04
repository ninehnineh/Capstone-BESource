using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetParkingHasPriceDetailWithPagination
{
    public class GetParkingHasPriceDetailWithPaginationResponse
    {
        public int ParkingHasPriceId { get; set; }
        public string? ParkingName { get; set; }
        public string ParkingPriceName { get; set; }

    }
}