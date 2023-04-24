using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class VehicleInfor
    {

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
