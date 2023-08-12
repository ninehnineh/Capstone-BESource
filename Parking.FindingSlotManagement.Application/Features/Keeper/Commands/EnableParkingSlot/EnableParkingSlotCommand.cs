using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.EnableParkingSlot
{
    public class EnableParkingSlotCommand : IRequest<ServiceResponse<string>>
    {
        public int ParkingSlotId { get; set; }
    }
}