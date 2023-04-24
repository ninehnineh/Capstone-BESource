using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string? Tiltle { get; set; }
        public string? Body { get; set; }
        public TimeSpan? SentTime { get; set; }
        public int? BookingId { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
