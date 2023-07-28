using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllConflictRequestByKeeperId
{
    public class GetAllConflictRequestByKeeperIdQuery : IRequest<ServiceResponse<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>>
    {
        public int KeeperId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
