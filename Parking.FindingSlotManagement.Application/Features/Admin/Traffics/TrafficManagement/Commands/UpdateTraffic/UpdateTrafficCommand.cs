using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.UpdateTraffic
{
    public class UpdateTrafficCommand : IRequest<ServiceResponse<string>>
    {
        public int TrafficId { get; set; }
        public string? Name { get; set; }
    }
}
