using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetListTraffic
{
    public class GetTrafficListQuery : IRequest<ServiceResponse<IEnumerable<GetListTrafficResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
