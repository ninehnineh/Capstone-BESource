using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public bool? IsActive { get; set; }
        public string? Devicetoken { get; set; }
        public bool? IsCensorship { get; set; }
        public int? ManagerId { get; set; }
        public int? RoleId { get; set; }

        public virtual User? Manager { get; set; }
        public virtual Role? Role { get; set; }
        public virtual BusinessProfile? BusinessProfile { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<FavoriteAddress> FavoriteAddresses { get; set; }
        public virtual ICollection<User> InverseManager { get; set; }
        public virtual ICollection<PayPal> PayPals { get; set; }
        public virtual ICollection<StaffParking> StaffParkings { get; set; }
        public virtual ICollection<VehicleInfor> VehicleInfors { get; set; }
        public virtual ICollection<VnPay> VnPays { get; set; }
    }
}
