using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking
{
    public class SearchRequestBookingQuery : IRequest<ServiceResponse<IEnumerable<SearchRequestBookingResponse>>>
    {
        public int KeeperId { get; set; }
        public string SearchString { get; set; }
    }
}
