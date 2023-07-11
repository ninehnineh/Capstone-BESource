using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Models.Transaction
{
    public class BookingTransactionDto
    {
        public int TransactionId { get; set; }
        public decimal Price { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
