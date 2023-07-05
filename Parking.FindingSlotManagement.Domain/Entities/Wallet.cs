using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public decimal Balance { get; set; }
        public decimal Debt { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Bill> Bills { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }

    }
}
