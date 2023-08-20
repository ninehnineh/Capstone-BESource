using Parking.FindingSlotManagement.Application.Models.Floor;
using Parking.FindingSlotManagement.Application.Models.Traffic;
using Parking.FindingSlotManagement.Application.Models.User;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetBookingDetails;

public class GetBookingDetailsResponse
{
    public BookingDetailsDto BookingDetails { get; set; }
    public UserBookingDto? User { get; set; }
    public VehicleInforDtoos? VehicleInfor { get; set; }
    public ParkingWithBookingDetailDto ParkingWithBookingDetailDto { get; set; }
    public ParkingSlotWithBookingDetailDto ParkingSlotWithBookingDetailDto { get; set; }
    public FloorWithBookingDetailDto FloorWithBookingDetailDto { get; set; }
    public List<TransactionWithBookingDetailDto> TransactionWithBookingDetailDtos { get; set; }
}

public class BookingDetailsDto
{
    public int BookingId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? CheckinTime { get; set; }
    public DateTime? CheckoutTime { get; set; }
    public string? Status { get; set; }
    public string? GuestName { get; set; }
    public string? GuestPhone { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? QRImage { get; set; }
    public bool? IsRating { get; set; }
}
public class VehicleInforDtoos
{
    public int VehicleInforId { get; set; }
    public string? LicensePlate { get; set; }
    public string? VehicleName { get; set; }
    public string? Color { get; set; }

}

public class ParkingWithBookingDetailDto
{
    public int ParkingId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
public class ParkingSlotWithBookingDetailDto
{
    public int ParkingSlotId { get; set; }
    public string? Name { get; set; }
}
public class FloorWithBookingDetailDto
{
    public int FloorId { get; set; }
    public string? FloorName { get; set; }
}
public class TransactionWithBookingDetailDto
{
    public int TransactionId { get; set; }
    public decimal Price { get; set; }
    public string? Status { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Description { get; set; }
}