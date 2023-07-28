using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Booking.BookingManagement.Queries.GetAllBookingForAdmin
{
    public class GetAllBookingForAdminQuery : IRequest<ServiceResponse<IEnumerable<GetAllBookingForAdminResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
