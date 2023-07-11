using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetRevenueByParkingId
{
    public class GetRevenueByParkingIdQuery: IRequest<ServiceResponse<IEnumerable<GetRevenueByParkingIdResponse>>>
    {
        public int ParkingId { get; set; }
        public string? Week { get; set; }
        public string? Month { get; set; }
    }
}
