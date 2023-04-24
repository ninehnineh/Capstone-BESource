using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class ParkingSlot
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public bool? IsAvailable { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
        public int? TrafficId { get; set; }
        public int? FloorId { get; set; }
        public int? ParkingId { get; set; }
        public int? BookingId { get; set; }

        public virtual Booking? Booking { get; set; }
        public virtual Floor? Floor { get; set; }
        public virtual Parking? Parking { get; set; }
        public virtual Traffic? Traffic { get; set; }
    }
}
