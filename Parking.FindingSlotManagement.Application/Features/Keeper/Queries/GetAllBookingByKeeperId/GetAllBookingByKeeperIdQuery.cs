using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllBookingByKeeperId
{
    public class GetAllBookingByKeeperIdQuery : IRequest<ServiceResponse<IEnumerable<GetAllBookingByKeeperIdResponse>>>
    {
        public int KeeperId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
