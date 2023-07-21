using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetCustomerActivities
{
    public class GetCustomerActivitiesQuery : IRequest<ServiceResponse<IEnumerable<GetCustomerActivitiesResponse>>>
    {
        public int UserId { get; set; }
    }
}
