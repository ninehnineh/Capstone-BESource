using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.DisableOrEnableParking
{
    public class DisableOrEnableParkingCommand : IRequest<ServiceResponse<string>>
    {
        public int ParkingId { get; set; }
    }
}
