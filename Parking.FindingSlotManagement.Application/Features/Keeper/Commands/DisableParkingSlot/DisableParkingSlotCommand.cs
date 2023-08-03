using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.ParkingSlots.Commands.DisableParkingSlot
{
    public class DisableParkingSlotCommand : IRequest<ServiceResponse<string>>
    {
        public int ParkingSlotId { get; set; }
        public string Reason { get; set; }
    }
}