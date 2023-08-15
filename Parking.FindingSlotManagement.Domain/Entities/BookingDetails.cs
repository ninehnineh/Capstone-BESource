using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class BookingDetails
    {
        public int BookingDetailsId { get; set; }
        public int? TimeSlotId { get; set; }
        public TimeSlot? TimeSlot { get; set; }
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
    }
}
