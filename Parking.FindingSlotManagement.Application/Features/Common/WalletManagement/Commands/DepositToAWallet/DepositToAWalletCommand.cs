using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.WalletManagement.Commands.DepositToAWallet
{
    public class DepositToAWalletCommand : IRequest<ServiceResponse<string>>
    {
        public decimal? TotalPrice { get; set; }
        public int? UserId { get; set; }
        public HttpContext? Context { get; set; }
    }
}
