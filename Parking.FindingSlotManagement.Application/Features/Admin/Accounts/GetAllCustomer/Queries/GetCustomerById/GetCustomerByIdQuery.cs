using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<ServiceResponse<GetCustomerByIdResponse>>
    {
        public int UserId { get; set; }
    }
}
