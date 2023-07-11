using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetNumberOfDoneAndCancelBookingByParkingId
{
    public class GetNumberOfDoneAndCancelBookingByParkingIdRes
    {
        public int NumberOfDoneBooking { get; set; }
        public int NumberOfCancelBooking { get; set; }
        public int Total { get; set; }
    }
}
