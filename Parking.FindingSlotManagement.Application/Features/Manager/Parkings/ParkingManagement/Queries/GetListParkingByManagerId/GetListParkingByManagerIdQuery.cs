using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByManagerId
{
    public class GetListParkingByManagerIdQuery : IRequest<ServiceResponse<IEnumerable<GetListParkingByManagerIdResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int ManagerId { get; set; }
    }
}
