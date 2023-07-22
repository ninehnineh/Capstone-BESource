using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Wallet.Queries.GetWalletByUserId
{
    public class GetWalletByUserIdQuery : IRequest<ServiceResponse<GetWalletByUserIdResponse>>
    {
        public int UserId { get; set; }
    }
}
