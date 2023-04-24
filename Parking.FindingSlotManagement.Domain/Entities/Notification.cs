using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string? Tiltle { get; set; }
        public string? Body { get; set; }
        public TimeSpan? SentTime { get; set; }

        public int? BookingId { get; set; }
        public virtual Booking? Booking { get; set; }
    }
}
