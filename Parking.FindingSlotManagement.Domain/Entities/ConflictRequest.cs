using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class ConflictRequest
    {
        public int ConflictRequestId { get; set; }
        public int ParkingId { get; set; }
        public string Message { get; set; }
        public int BookingId { get; set; }
        public string Status { get; set; }
    }
}