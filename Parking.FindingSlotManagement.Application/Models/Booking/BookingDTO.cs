using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Models.Booking
{
    public class BookingDTO
    {
        public TimeSpan StartTime { get; set; }
        public DateTime DateBook { get; set; }
        public int ParkingSlotId { get; set; }
    }
}
