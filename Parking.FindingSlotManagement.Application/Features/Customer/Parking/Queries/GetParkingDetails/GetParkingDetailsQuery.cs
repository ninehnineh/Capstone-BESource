using MediatR;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetParkingDetails
{
    public class GetParkingDetailsQuery : IRequest<ServiceResponse<GetParkingDetailsResponse>>
    {
        public int ParkingId { get; set; }

    }
}
