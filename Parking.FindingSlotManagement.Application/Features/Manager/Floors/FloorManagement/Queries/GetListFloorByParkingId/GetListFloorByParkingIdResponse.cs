using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloorByParkingId
{
    public class GetListFloorByParkingIdResponse
    {
        public int FloorId { get; set; }
        public string? FloorName { get; set; }
    }
}
