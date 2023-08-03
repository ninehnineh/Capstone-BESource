using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Models.ParkingSlot
{
    public class DisableSlotResult
    {
        public int BookingId { get; set; }
        public Domain.Entities.Wallet Wallet { get; set; }
        public Domain.Entities.User User { get; set; }
    }
}