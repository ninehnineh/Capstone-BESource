using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetUpcommingBooking
{
    public class GetUpcommingBookingResponse
    {
        public BookingSearchResult BookingSearchResult { get; set; }
        public VehicleInforSearchResult VehicleInforSearchResult { get; set; }
        public ParkingSearchResult ParkingSearchResult { get; set; }
        public ParkingSlotSearchResult ParkingSlotSearchResult { get; set; }
    }
}
