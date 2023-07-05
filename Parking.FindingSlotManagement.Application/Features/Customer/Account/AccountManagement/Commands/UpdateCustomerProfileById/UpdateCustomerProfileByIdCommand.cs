using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Commands.UpdateCustomerProfileById
{
    public class UpdateCustomerProfileByIdCommand : IRequest<ServiceResponse<string>>
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
    }
}
