using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetBookingById
{
    public class GetBookingByIdResponse
    {
        public int BookingId { get; set; }
        public DateTime DateBook { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAvatar { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
        public decimal UnPaidMoney { get; set; }
        public decimal? TotalPrice { get; set; }
        public string ParkingSlotName { get; set; }
        public string? FloorName { get; set; }
        public int ParkingId { get; set; }
        public string ParkingName { get; set; }
        public string? LicensePlate { get; set; }
        public string? VehicleName { get; set; }
        public string? Color { get; set; }
        /*public string TrafficName { get; set; } */
    }
}
