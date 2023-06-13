using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetListKeeperByManagerId
{
    public class GetListKeeperByManagerIdQuery : IRequest<ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int ManagerId { get; set; }
    }
}
