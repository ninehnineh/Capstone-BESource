using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class BookedSlot
    {
        public int BookedSlotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int? ParkingSlotId { get; set; }
        public ParkingSlot? Parkingslot { get; set; }
    }
}
