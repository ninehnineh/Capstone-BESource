using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetAvailableSlots
{
    public class GetAvailableSlotsResponse
    {
        public int ParkingSlotId { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public int TrafficId { get; set; }
        public int FloorId { get; set; }
    }
}
