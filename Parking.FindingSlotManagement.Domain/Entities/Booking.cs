using Firebase.Auth;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class Booking 
    {
        public TimeSpan StartTime { get; set; }
        public DateTime DateBook { get; set; }
        public int ParkingSlotId { get; set; }
        public int BookingId { get; set; }
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



        public int? UserId { get; set; }
        public virtual User? User { get; set; }
        public int? VehicleInforId { get; set; }
        public virtual VehicleInfor? VehicleInfor { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        public virtual ICollection<ParkingSlot> ParkingSlots { get; set; }
    }
}
