using Parking.FindingSlotManagement.Application.Features.Admin.Booking.BookingManagement.Queries.GetAllBookingForAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetAllBookingByParkingId
{
    public class GetAllBookingByParkingIdResponse
    {
        public BookingForGetAllBookingByParkingIdResponse BookingForGetAllBookingByParkingIdResponse { get; set; }
        public ParkingDtoForAdmin ParkingDtoForAdmin { get; set; }
        public FloorDtoForAdmin FloorDtoForAdmin { get; set; }
        public SlotDtoForAdmin SlotDtoForAdmin { get; set; }
        public VehicleForGetAllBookingByParkingIdResponse VehicleForGetAllBookingByParkingIdResponse { get; set; }
        public UserForGetAllBookingByParkingIdResponse UserForGetAllBookingByParkingIdResponse { get; set; }
    }
    public class BookingForGetAllBookingByParkingIdResponse
    {
        public int BookingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public string? Status { get; set; }
        public decimal? TotalPrice { get; set; }
    }
    public class VehicleForGetAllBookingByParkingIdResponse
    {
        public int VehicleInforId { get; set; }
        public string? LicensePlate { get; set; }
    }
    public class UserForGetAllBookingByParkingIdResponse
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
    }
}
