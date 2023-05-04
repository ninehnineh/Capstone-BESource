using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.NonCensorshipManagerAccount.Queries.GetNonCensorshipManagerAccountList
{
    public class GetNonCensorshipManagerAccountListQuery : IRequest<ServiceResponse<IEnumerable<NonCensorshipManagerAccountResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
