using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class Bill
    {
        public int BillId { get; set; }
        public DateTime Time { get; set; }
        public string? Status { get; set; }
        public decimal Price { get; set; }

        public int? BusinessId { get; set; }
        public BusinessProfile? businessProfile { get; set; }
        public int? WalletId { get; set; }
        public Wallet? Wallet { get; set; }
    }
}
