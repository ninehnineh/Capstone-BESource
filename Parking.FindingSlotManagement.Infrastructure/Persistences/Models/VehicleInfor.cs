using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class VehicleInfor
    {
        public VehicleInfor()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string? LicensePlate { get; set; }
        public string? VehicleName { get; set; }
        public string? Color { get; set; }
        public int? UserId { get; set; }
        public int? TrafficId { get; set; }

        public virtual Traffic? Traffic { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
