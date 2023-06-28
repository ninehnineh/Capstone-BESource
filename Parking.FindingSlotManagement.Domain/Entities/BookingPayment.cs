using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class BookingPayment
    {
        public int BookingPaymentId { get; set; }
        public decimal Price { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }

        public int? WalletId { get; set; }
        public Wallet? Wallet { get; set; }
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }

    }
}
