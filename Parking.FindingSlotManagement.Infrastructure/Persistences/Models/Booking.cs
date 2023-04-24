using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Notifications = new HashSet<Notification>();
            ParkingSlots = new HashSet<ParkingSlot>();
        }

        public int ParkingSlotId { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime DateBook { get; set; }
        public int Id { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? CheckinTime { get; set; }
        public TimeSpan? CheckoutTime { get; set; }
        public decimal? ActualPrice { get; set; }
        public string? QrcodeText { get; set; }
        public int? Status { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TmnCodeVnPay { get; set; }
        public int? VehicleInforId { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual VehicleInfor? VehicleInfor { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<ParkingSlot> ParkingSlots { get; set; }
    }
}
