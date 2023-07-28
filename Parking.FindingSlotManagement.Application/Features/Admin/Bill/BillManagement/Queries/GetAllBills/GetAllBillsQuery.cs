using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Bill.BillManagement.Queries.GetAllBills
{
    public class GetAllBillsQuery : IRequest<ServiceResponse<IEnumerable<GetAllBillsResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
