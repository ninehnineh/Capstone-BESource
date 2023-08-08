using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking
{
    public class SearchRequestBookingResponse
    {
        public BookingSearchResult BookingSearchResult { get; set; }
        public VehicleInforSearchResult VehicleInforSearchResult { get; set; }
        public ParkingSearchResult ParkingSearchResult { get; set; }
        public ParkingSlotSearchResult ParkingSlotSearchResult { get; set; }
    }
    public class BookingSearchResult
    {
        public int BookingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime DateBook { get; set; }
        public string? Status { get; set; }
        public bool? IsRating { get; set; }
    }
    public class VehicleInforSearchResult
    {
        public int VehicleInforId { get; set; }
        public string? LicensePlate { get; set; }
        public string? VehicleName { get; set; }
        public int? TrafficId { get; set; }
        public string TrafficName { get; set; }
    }
    public class ParkingSearchResult
    {
        public int ParkingId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
    public class ParkingSlotSearchResult
    {
        public int ParkingSlotId { get; set; }
        public string? Name { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
        public string FloorName { get; set; }
    }
}
