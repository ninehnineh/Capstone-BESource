using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetPaypalByManagerId
{
    public class GetPaypalByManagerIdQuery : IRequest<ServiceResponse<GetPaypalByManagerIdResponse>>
    {
        public int ManagerId { get; set; }
    }
}
