using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.CreateNewVnPay
{
    public class CreateNewVnPayCommand : IRequest<ServiceResponse<int>>
    {
        public string? TmnCode { get; set; }
        public string? HashSecret { get; set; }
        public int? UserId { get; set; }
    }
}
