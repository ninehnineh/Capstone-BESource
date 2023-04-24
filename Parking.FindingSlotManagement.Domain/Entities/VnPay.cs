using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class VnPay
    {
        public int Id { get; set; }
        public string? TmnCode { get; set; }
        public string? HashSecret { get; set; }
        public int? ManagerId { get; set; }

        public virtual User? Manager { get; set; }
    }
}
