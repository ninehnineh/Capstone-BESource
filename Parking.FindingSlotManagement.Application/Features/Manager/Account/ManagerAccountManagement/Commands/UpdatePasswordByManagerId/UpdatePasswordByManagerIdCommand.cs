using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Account.ManagerAccountManagement.Commands.UpdatePasswordByManagerId
{
    public class UpdatePasswordByManagerIdCommand : IRequest<ServiceResponse<string>>
    {
        public int ManagerId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
