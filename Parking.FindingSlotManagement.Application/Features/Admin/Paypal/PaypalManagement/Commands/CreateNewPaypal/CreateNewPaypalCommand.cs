using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.CreateNewPaypal
{
    public class CreateNewPaypalCommand : IRequest<ServiceResponse<int>>
    {
        public string? ClientId { get; set; }
        public string? SecretKey { get; set; }
        public int? ManagerId { get; set; }
    }
}
