using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Commands.DeleteVehicleInfoForGuest
{
    public class DeleteVehicleInforForGuestCommand : IRequest<ServiceResponse<string>>
    {
        public int VehicleInforId { get; set; }
    }
}
