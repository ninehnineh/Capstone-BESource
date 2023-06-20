using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloorByParkingId
{
    public class GetListFloorByParkingIdQuery : IRequest<ServiceResponse<IEnumerable<GetListFloorByParkingIdResponse>>>
    {
        public int ParkingId { get; set; }
    }
}
