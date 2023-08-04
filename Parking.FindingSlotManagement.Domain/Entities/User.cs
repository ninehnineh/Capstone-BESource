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
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public bool? IsActive { get; set; }
        public string? Devicetoken { get; set; }
        public bool? IsCensorship { get; set; }
        public int? ManagerId { get; set; }
        public int? RoleId { get; set; }
        public string? IdCardNo { get; set; }
        public DateTime? IdCardDate { get; set; }
        public string? IdCardIssuedBy { get; set; }
        public string? Address { get; set; }
        public int? BanCount { get; set; }

        public virtual User? Manager { get; set; }
        public virtual Role? Role { get; set; }
        public Wallet? Wallet { get; set; }
        public virtual BusinessProfile? BusinessProfile { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<FavoriteAddress> FavoriteAddresses { get; set; }
        public virtual ICollection<User> InverseManager { get; set; }
        public virtual ICollection<PayPal> PayPals { get; set; }
        public virtual ICollection<VehicleInfor> VehicleInfors { get; set; }
        public int? ParkingId { get; set; }
        public Parking? Parking { get; set; }
        public virtual ICollection<VnPay> VnPays { get; set; }
        public virtual ICollection<ApproveParking> ApproveParkings { get; set; }

    }
}
