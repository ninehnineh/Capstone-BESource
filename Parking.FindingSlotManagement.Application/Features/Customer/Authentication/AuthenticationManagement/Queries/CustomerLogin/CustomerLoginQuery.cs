using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Queries.CustomerLogin
{
    public class CustomerLoginQuery : IRequest<ServiceResponse<string>>
    {
        public string Phone { get; set; }
    }
}
