using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.Accounts.StaffAccountManagement.Commands.UpdatePasswordByStaffId
{
    public class UpdatePasswordByStaffIdCommand : IRequest<ServiceResponse<string>>
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
