using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CancelDisableScheduledParking
{
    public class CancelDisableScheduledParkingCommand : IRequest<ServiceResponse<string>>
    {
        public int ParkingId { get; set; }
        public DateTime DisableDate { get; set; }
    }
}