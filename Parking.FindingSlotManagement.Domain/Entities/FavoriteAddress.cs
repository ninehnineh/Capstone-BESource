using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class FavoriteAddress
    {
        public int Id { get; set; }
        public string? TagName { get; set; }
        public string? Address { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
