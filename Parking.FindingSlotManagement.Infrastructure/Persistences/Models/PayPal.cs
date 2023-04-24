using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class PayPal
    {
        public int Id { get; set; }
        public string? ClientId { get; set; }
        public string? SecretKey { get; set; }
        public int? ManagerId { get; set; }

        public virtual User? Manager { get; set; }
    }
}
