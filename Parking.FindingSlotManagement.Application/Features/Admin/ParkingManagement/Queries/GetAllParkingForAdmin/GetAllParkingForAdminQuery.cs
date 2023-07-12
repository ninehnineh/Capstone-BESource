using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ParkingManagement.Queries.GetAllParkingForAdmin
{
    public class GetAllParkingForAdminQuery : IRequest<ServiceResponse<IEnumerable<GetAllParkingForAdminResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
