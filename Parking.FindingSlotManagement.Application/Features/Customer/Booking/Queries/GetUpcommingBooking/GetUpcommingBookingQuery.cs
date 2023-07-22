using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetUpcommingBooking
{
    public class GetUpcommingBookingQuery : IRequest<ServiceResponse<IEnumerable<GetUpcommingBookingResponse>>>
    {
        public int UserId { get; set; }
    }
}
