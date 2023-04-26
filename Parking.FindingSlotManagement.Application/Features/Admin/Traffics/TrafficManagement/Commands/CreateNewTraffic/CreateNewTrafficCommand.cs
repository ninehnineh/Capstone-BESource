using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.CreateNewTraffic
{
    public class CreateNewTrafficCommand : IRequest<ServiceResponse<int>>
    {
        public string? Name { get; set; }
    }
}
