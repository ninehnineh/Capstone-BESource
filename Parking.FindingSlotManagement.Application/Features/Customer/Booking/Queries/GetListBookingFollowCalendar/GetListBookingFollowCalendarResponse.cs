using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetListBookingFollowCalendar
{
    public class GetListBookingFollowCalendarResponse
    {
        public DateTime StartTime { get; set; }
        public DateTime DateBook { get; set; }
        public int ParkingSlotId { get; set; }
        public int BookingId { get; set; }
        public DateTime? EndTime { get; set; }
        public int? ParkingId { get; set; }
    }
}
