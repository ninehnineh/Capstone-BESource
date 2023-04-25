using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Queries.GetCensorshipManagerAccountList
{
    public class GetCensorshipManagerAccountListQuery : IRequest<ServiceResponse<IEnumerable<CensorshipManagerAccountResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
