using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Models.Parking
{
    public class DisableSlotParking
    {
        public int ParkingId { get; set; }
        public int ManagerId { get; set; }
    }
}