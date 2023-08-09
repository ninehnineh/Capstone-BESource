using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Models.Parking
{
    public class DisableParkingHistory
    {
        public int ParkingId { get; set; }
        public DateTime DisableDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Reason { get; set; }
    }
}