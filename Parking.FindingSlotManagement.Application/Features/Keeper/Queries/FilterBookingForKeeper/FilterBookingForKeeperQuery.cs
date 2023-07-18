using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Queries.FilterBookingForKeeper
{
    public class FilterBookingForKeeperQuery : IRequest<ServiceResponse<IEnumerable<FilterBookingForKeeperResponse>>>
    {
        public DateTime? Date { get; set; }
        public string? Status { get; set; }
        public int KeeperId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
