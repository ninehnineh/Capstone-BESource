using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class VnPay
    {
        public int Id { get; set; }
        public string? TmnCode { get; set; }
        public string? HashSecret { get; set; }
        public int? ManagerId { get; set; }

        public virtual User? Manager { get; set; }
    }
}
