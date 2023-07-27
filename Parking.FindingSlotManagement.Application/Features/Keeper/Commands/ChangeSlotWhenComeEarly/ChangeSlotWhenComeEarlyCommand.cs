using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.ChangeSlotWhenComeEarly
{
    public class ChangeSlotWhenComeEarlyCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }
        public int ParkingSlotId { get; set; }
    }
}
