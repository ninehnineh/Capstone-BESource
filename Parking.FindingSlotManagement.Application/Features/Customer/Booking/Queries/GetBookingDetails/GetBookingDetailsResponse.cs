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
    public VehicleInforDto? VehicleInfor { get; set; }
    public BookedParkingSlotDto ParkingSlot { get; set; }
}

public class BookingDetailsDto
{
    public DateTime StartTime { get; set; }
    public DateTime DateBook { get; set; }
    public int BookingId { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal? ActualPrice { get; set; }
    public string? Status { get; set; }
    public string? GuestName { get; set; }
    public string? GuestPhone { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TmnCodeVnPay { get; set; }
    public string? QRImage { get; set; }
}
public class VehicleInforDto
{
    public int VehicleInforId { get; set; }
    public string? LicensePlate { get; set; }
    public string? VehicleName { get; set; }
    public string? Color { get; set; }

}

public class BookedParkingSlotDto
{
    public int ParkingSlotId { get; set; }
    public string? Name { get; set; }
    public TrafficDto? Traffic { get; set; }
    public FloorDto? Floor { get; set; }
}
