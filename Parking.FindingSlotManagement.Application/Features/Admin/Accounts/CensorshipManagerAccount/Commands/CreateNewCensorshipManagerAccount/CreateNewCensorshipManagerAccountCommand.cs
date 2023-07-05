using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.CreateNewCensorshipManagerAccount
{
    public class CreateNewCensorshipManagerAccountCommand : IRequest<ServiceResponse<int>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsCensorship { get; set; } = true;
        public int? RoleId { get; set; } = 1;
        public int? ParkingId { get; set; }
    }
}
