using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListApproveParkingByParkingId
{
    public class GetListApproveParkingByParkingIdRes
    {
        public int ApproveParkingId { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public int? StaffId { get; set; }
        public string StaffName { get; set; }
        public int ParkingId { get; set; }
    }
}
