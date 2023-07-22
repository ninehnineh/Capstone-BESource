using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Wallet.Queries.GetWalletByUserId
{
    public class GetWalletByUserIdResponse
    {
        public int WalletId { get; set; }
        public decimal Balance { get; set; }
        public decimal Debt { get; set; }
    }
}
