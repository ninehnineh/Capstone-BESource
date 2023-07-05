using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Models.BookingDetails
{
    public class CreateCreateBookingDetailsDto
    {
        public int BookingDetailsId { get; set; }
        public int? TimeSlotId { get; set; }
        public int? BookingId { get; set; }

    }
}
