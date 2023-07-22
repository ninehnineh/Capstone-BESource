using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.CreateBookingForPasserby
{
    public class CreateBookingForPasserbyCommand : IRequest<ServiceResponse<int>>
    {
        public BookingForPasserby BookingForPasserby { get; set; }
        public VehicleInformationForPasserby VehicleInformationForPasserby { get; set; }
    }
    public class BookingForPasserby 
    {
        public int ParkingSlotId { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime DateBook { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
    }
    public class VehicleInformationForPasserby
    {
        public string? LicensePlate { get; set; }
        public string? VehicleName { get; set; }
        public string? Color { get; set; }
        public int? TrafficId { get; set; }
    }
}
