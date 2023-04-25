using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.DeleteCensorshipManagerAccount
{
    public class DisableOrEnableManagerAccountCommand : IRequest<ServiceResponse<string>>
    {
        public int UserId { get; set; }
    }
}
