using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetListBookingByManagerId
{
    public class GetListBookingByManagerIdResponse
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAvatar { get; set; }
        public string? Phone { get; set; }
        public string Position { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? ActualPrice { get; set; }
        public string? LicensePlate { get; set; }
        public string ParkingName { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public string? PaymentMethod { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
        public string? Status { get; set; }
    }
}
