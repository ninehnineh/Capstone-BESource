using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetAllBookingByParkingId
{
    public class GetAllBookingByParkingIdQuery : IRequest<ServiceResponse<IEnumerable<GetAllBookingByParkingIdResponse>>>
    {
        public int ParkingId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
