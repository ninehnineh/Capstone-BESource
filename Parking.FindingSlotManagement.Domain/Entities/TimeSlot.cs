using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public int? ParkingSlotId { get; set; }
        public ParkingSlot? Parkingslot { get; set; }
        public ICollection<BookingDetails>? BookingDetails { get; set; }
    }
}
