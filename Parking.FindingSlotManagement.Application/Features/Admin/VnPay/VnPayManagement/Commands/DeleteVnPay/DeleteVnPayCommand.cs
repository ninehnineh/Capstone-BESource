using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.DeleteVnPay
{
    public class DeleteVnPayCommand : IRequest<ServiceResponse<string>>
    {
        public int VnPayId { get; set; }
    }
}
