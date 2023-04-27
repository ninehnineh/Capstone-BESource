using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.DisableOrEnableFloor
{
    public class DisableOrEnableFloorCommand : IRequest<ServiceResponse<string>>
    {
        public int FloorId { get; set; }
    }
}
