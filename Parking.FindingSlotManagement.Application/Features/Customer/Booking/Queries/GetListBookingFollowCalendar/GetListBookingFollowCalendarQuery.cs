using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetListBookingFollowCalendar
{
    public class GetListBookingFollowCalendarQuery : IRequest<ServiceResponse<IEnumerable<GetListBookingFollowCalendarResponse>>>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
