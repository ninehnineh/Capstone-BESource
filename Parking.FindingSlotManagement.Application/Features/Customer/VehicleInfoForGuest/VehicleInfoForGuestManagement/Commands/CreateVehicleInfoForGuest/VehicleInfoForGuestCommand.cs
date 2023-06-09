using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Commands.CreateVehicleInfoForGuest
{
    public class VehicleInfoForGuestCommand : IRequest<ServiceResponse<int>>
    {
        public string? LicensePlate { get; set; }
        public string? VehicleName { get; set; }
        public string? Color { get; set; }
        public int? TrafficId { get; set; }
    }
}
