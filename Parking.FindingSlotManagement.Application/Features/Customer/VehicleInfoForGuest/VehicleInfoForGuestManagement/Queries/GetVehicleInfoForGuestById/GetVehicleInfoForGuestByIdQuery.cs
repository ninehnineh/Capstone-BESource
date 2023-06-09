using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Queries.GetVehicleInfoForGuestById
{
    public class GetVehicleInfoForGuestByIdQuery : IRequest<ServiceResponse<GetVehicleInfoForGuestByIdResponse>>
    {
        public int VehicleInforId { get; set; }
    }
}
