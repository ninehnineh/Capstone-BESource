using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Queries.GetCustomerProfileById
{
    public class GetCustomerProfileByIdQuery : IRequest<ServiceResponse<GetCustomerProfileByIdResponse>>
    {
        public int UserId { get; set; }
    }
}
