using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByBusinessId
{
    public class GetVnPayByBusinessIdQuery : IRequest<ServiceResponse<GetVnPayByBusinessIdResponse>>
    {
        public int BusinessId { get; set; }
    }
}
