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
        public DateTime StartTime { get; set; }
        public DateTime DateBook { get; set; }
        public int ParkingSlotId { get; set; }
        public int BookingId { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public string? Status { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? QRImage { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }
        public int? VehicleInforId { get; set; }
        public virtual VehicleInfor? VehicleInfor { get; set; }
        public ParkingSlot ParkingSlot { get; set; }
        public ICollection<BookingPayment> BookingPayments { get; set; }
    }
}
