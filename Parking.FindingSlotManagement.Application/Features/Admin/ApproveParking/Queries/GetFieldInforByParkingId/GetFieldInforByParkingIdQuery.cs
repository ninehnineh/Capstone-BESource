using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetFieldInforByParkingId
{
    public class GetFieldInforByParkingIdQuery : IRequest<ServiceResponse<IEnumerable<GetFieldInforByParkingIdResponse>>>
    {
        public int ParkingId { get; set; }
    }
}
