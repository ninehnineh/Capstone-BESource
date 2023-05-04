using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.UpdateFloor
{
    public class UpdateFloorCommand : IRequest<ServiceResponse<string>>
    {
        public int FloorId { get; set; }
        public string? FloorName { get; set; }
        public int? ParkingId { get; set; }
    }
}
