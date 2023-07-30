using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayById
{
    public class GetVnPayByIdQuery : IRequest<ServiceResponse<GetVnPayByIdResponse>>
    {
        public int VnPayId { get; set; }
    }
}
