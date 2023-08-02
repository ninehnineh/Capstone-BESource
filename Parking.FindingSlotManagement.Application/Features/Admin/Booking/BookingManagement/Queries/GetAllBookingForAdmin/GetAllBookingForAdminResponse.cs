using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Booking.BookingManagement.Queries.GetAllBookingForAdmin
{
    public class GetAllBookingForAdminResponse
    {
        public BookingDtoForAdmin BookingDtoForAdmin { get; set; }
        public ParkingDtoForAdmin ParkingDtoForAdmin { get; set; }
        public FloorDtoForAdmin FloorDtoForAdmin { get; set; }
        public SlotDtoForAdmin SlotDtoForAdmin { get; set; }
        public UserForGetAllBookingForAdminResponse UserForGetAllBookingForAdminResponse { get; set; }
    }
    public class BookingDtoForAdmin
    {
        public int BookingId { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public string? Status { get; set; }
        public decimal? TotalPrice { get; set; }
    }
    public class ParkingDtoForAdmin
    {
        public int ParkingId { get; set; }
        public string? Name { get; set; }
    }
    public class FloorDtoForAdmin
    {
        public int FloorId { get; set; }
        public string? FloorName { get; set; }
    }
    public class SlotDtoForAdmin
    {
        public int ParkingSlotId { get; set; }
        public string? Name { get; set; }
    }
    public class UserForGetAllBookingForAdminResponse
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
    }
}
