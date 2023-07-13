using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListParkingNewWNoApprove
{
    public class GetListParkingNewWNoApproveQuery : IRequest<ServiceResponse<IEnumerable<GetListParkingNewWNoApproveResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
