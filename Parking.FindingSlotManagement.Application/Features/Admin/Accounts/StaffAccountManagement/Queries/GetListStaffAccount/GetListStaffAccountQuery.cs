using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Queries.GetListStaffAccount
{
    public class GetListStaffAccountQuery : IRequest<ServiceResponse<IEnumerable<GetListStaffAccountResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
