using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Queries
{
    public class GetRequestAccountListQuery : IRequest<ServiceResponse<IEnumerable<RequestResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
