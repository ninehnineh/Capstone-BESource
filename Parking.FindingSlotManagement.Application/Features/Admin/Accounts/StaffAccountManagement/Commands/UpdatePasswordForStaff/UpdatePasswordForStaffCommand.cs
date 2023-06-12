using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.UpdatePasswordForStaff
{
    public class UpdatePasswordForStaffCommand : IRequest<ServiceResponse<string>>
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
