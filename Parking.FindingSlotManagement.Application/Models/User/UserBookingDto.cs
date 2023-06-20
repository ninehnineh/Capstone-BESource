using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Models.User
{
    public class UserBookingDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
    }
}
