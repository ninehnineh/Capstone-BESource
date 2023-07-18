using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Domain.Entities
{
    public class ApproveParking
    {
        public int ApproveParkingId { get; set; }
        public string? Note { get; set; }
        public string? NoteForAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public int? StaffId { get; set; }
        public User? User { get; set; }
        public int? ParkingId { get; set; }
        public Parking? Parking { get; set; }
        public ICollection<FieldWorkParkingImg> FieldWorkParkingImgs { get; set; }

    }
}
