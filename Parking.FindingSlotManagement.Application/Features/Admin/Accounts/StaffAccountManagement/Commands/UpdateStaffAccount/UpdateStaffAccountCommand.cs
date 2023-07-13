using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.UpdateStaffAccount
{
    public class UpdateStaffAccountCommand : IRequest<ServiceResponse<string>>
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
    }
}
