using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByUserId
{
    public class GetVnPayByUserIdQuery : IRequest<ServiceResponse<GetVnPayByUserIdResponse>>
    {
        public int UserId { get; set; }
    }
}
