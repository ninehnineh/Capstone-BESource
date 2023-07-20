using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.TransactionManagement.Commands.CreateNewTransaction
{
    public class CreateNewTransactionCommand : IRequest<ServiceResponse<int>>
    {
        public decimal? Price { get; set; }
        public string? Status { get; set; }
        public int? WalletId { get; set; }
        public string? Description { get; set; }
    }
}
