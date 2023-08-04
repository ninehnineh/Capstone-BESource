using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class ParkingSlot
    {
        public int ParkingSlotId { get; set; }
        public string? Name { get; set; }
        public bool? IsAvailable { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
        public int? TrafficId { get; set; }
        public int? FloorId { get; set; }
        public bool? IsBackup { get; set; }

        public virtual ICollection<TimeSlot> TimeSlots { get; set; }
        public virtual Floor? Floor { get; set; }
        public virtual Traffic? Traffic { get; set; }
    }
}
