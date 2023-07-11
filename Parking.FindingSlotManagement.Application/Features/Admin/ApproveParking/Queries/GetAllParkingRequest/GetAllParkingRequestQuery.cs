using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetAllParkingRequest
{
    public class GetAllParkingRequestQuery : IRequest<ServiceResponse<IEnumerable<GetAllParkingRequestResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
