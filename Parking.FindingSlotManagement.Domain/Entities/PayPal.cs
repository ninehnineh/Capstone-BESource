using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class PayPal
    {
        public int PayPalId { get; set; }
        public string? ClientId { get; set; }
        public string? SecretKey { get; set; }
        public int? ManagerId { get; set; }

        public virtual User? Manager { get; set; }
    }
}
