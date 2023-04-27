using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Queries.GetVehicleInforById
{
    public class GetVehicleInforByIdQuery : IRequest<ServiceResponse<GetVehicleInforByIdResponse>>
    {
        public int VehicleInforId { get; set; }
    }
}
