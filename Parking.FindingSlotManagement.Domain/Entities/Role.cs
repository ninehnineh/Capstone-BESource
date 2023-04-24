using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
