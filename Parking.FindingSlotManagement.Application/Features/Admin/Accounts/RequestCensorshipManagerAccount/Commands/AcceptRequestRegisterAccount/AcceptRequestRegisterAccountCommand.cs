using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Commands.AcceptRequestRegisterAccount
{
    public class AcceptRequestRegisterAccountCommand : IRequest<ServiceResponse<string>>
    {
        public int UserId { get; set; }
    }
}
