using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByManagerId
{
    public class GetVnPayByManagerIdQuery : IRequest<ServiceResponse<GetVnPayByManagerIdResponse>>
    {
        public int ManagerId { get; set; }
    }
}
