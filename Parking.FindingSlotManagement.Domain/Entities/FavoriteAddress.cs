using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class FavoriteAddress
    {
        public int FavoriteAddressId { get; set; }
        public string? TagName { get; set; }
        public string? Address { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
