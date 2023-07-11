using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetBookingDetails
{
    public class GetBookingDetailsQuery : IRequest<ServiceResponse<GetBookingDetailsResponse>>
    {
        public int BookingId { get; set; }
    }
}
