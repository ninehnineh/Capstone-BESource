using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetNumberOfDoneAndCancelBooking
{
    public class GetNumberOfDoneAndCancelBookingQuery : IRequest<ServiceResponse<GetNumberOfDoneAndCancelBookingRes>>
    {
        public int ManagerId { get; set; }
    }
}
