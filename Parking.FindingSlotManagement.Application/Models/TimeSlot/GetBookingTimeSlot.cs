using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Models.TimeSlot
{
    public record class GetBookingTimeSlot
    {

        public int TimeSlotId { get; set; }
        public string Status { get; set; }

    }
}
