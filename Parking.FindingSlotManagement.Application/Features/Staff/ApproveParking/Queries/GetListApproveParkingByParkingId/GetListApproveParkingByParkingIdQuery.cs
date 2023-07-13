using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListApproveParkingByParkingId
{
    public class GetListApproveParkingByParkingIdQuery : IRequest<ServiceResponse<IEnumerable<GetListApproveParkingByParkingIdRes>>>
    {
        public int ParkingId { get; set; }
    }
}
