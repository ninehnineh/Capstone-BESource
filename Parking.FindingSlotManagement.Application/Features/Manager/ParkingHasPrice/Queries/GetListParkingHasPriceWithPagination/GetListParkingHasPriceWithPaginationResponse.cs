using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetListParkingHasPriceWithPagination
{
    public class GetListParkingHasPriceWithPaginationResponse
    {
        public int ParkingHasPriceId { get; set; }
        public string? ParkingName { get; set; }
        public string ParkingPriceName { get; set; }
    }
}