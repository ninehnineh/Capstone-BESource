using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.DisableParkingByDate
{
    public class DisableParkingByDateCommand : IRequest<ServiceResponse<string>>
    {   
        public int ParkingId { get; set; }
        // public int ParkingSlotId { get; set; }
        public string Reason { get; set; }
        public DateTime DisableDate { get; set; }

    }
}