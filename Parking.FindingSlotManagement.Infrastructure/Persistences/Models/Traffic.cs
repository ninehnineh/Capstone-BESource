using System;
using System.Collections.Generic;

namespace Parking.FindingSlotManagement.Infrastructure.Persistences.Models
{
    public partial class Traffic
    {
        public Traffic()
        {
            PackagePrices = new HashSet<PackagePrice>();
            ParkingSlots = new HashSet<ParkingSlot>();
            VehicleInfors = new HashSet<VehicleInfor>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<PackagePrice> PackagePrices { get; set; }
        public virtual ICollection<ParkingSlot> ParkingSlots { get; set; }
        public virtual ICollection<VehicleInfor> VehicleInfors { get; set; }
    }
}
