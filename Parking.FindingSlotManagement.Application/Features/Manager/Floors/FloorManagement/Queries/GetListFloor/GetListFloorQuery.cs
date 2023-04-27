using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloor
{
    public class GetListFloorQuery : IRequest<ServiceResponse<IEnumerable<GetListFloorResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
