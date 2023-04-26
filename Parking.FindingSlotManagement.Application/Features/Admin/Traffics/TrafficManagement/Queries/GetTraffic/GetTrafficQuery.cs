using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetTraffic
{
    public class GetTrafficQuery : IRequest<ServiceResponse<GetTrafficResponse>>
    {
        public int TrafficId { get; set; }
    }
}
