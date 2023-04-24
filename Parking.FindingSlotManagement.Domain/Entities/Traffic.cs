using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class Traffic
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<PackagePrice> PackagePrices { get; set; }
        public virtual ICollection<ParkingSlot> ParkingSlots { get; set; }
        public virtual ICollection<VehicleInfor> VehicleInfors { get; set; }
    }
}
