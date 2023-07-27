using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Bill.BillManagement.Queries.GetAllBills
{
    public class GetAllBillsResponse
    {
        public int BillId { get; set; }
        public DateTime Time { get; set; }
        public string? Status { get; set; }
        public decimal Price { get; set; }

        public int? BusinessId { get; set; }
        public string BusinessName { get; set; }
    }
}
