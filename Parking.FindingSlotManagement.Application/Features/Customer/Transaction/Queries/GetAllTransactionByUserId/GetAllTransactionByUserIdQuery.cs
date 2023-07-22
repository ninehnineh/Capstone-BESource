using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Transaction.Queries.GetAllTransactionByUserId
{
    public class GetAllTransactionByUserIdQuery : IRequest<ServiceResponse<IEnumerable<GetAllTransactionByUserIdResponse>>>
    {
        public int UserId { get; set; }
    }
}
