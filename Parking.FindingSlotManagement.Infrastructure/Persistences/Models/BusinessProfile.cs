using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class BusinessProfile
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? FrontIdentification { get; set; }
        public string? BackIdentification { get; set; }
        public string? BusinessLicense { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
