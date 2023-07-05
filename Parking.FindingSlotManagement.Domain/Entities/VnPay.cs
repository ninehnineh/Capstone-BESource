using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class VnPay
    {
        public int VnPayId { get; set; }
        public string? TmnCode { get; set; }
        public string? HashSecret { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

    }
}
