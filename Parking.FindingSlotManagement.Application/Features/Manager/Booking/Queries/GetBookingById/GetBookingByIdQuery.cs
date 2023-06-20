using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetBookingById
{
    public class GetBookingByIdQuery : IRequest<ServiceResponse<GetBookingByIdResponse>>
    {
        public int BookingId { get; set; }
    }
}
