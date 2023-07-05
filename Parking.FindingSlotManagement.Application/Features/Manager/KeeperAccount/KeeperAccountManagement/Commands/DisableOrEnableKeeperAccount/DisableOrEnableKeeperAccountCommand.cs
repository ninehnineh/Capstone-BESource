using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Commands.DisableOrEnableKeeperAccount
{
    public class DisableOrEnableKeeperAccountCommand : IRequest<ServiceResponse<string>>
    {
        public int UserId { get; set; }
    }
}
