using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetListBookingByManagerId
{
    public class GetListBookingByManagerIdQuery : IRequest<ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int ManagerId { get; set; }
    }
}
