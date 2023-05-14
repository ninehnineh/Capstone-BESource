using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.DeletePaypal
{
    public class DeletePaypalCommand : IRequest<ServiceResponse<string>>
    {
        public int PayPalId { get; set; }
    }
}
