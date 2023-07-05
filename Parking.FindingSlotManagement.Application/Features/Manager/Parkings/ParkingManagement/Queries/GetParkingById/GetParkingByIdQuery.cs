using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetParkingById
{
    public class GetParkingByIdQuery : IRequest<ServiceResponse<GetParkingByIdResponse>>
    {
        public int ParkingId { get; set; }
    }
}
