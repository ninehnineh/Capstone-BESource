using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetListPaypal
{
    public class GetListPaypalQuery : IRequest<ServiceResponse<IEnumerable<GetListPaypalResponse>>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
