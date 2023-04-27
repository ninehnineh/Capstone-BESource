using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Queries.GetListVehicleInforByUserId
{
    public class GetListVehicleInforByUserIdQuery : IRequest<ServiceResponse<IEnumerable<GetListVehicleInforByUserIdResponse>>>
    {
        public int UserId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
