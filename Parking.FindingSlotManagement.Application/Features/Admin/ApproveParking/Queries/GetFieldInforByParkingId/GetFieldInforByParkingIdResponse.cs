using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetFieldInforByParkingId
{
    public class GetFieldInforByParkingIdResponse
    {
        public int ApproveParkingId { get; set; }
        public string? Note { get; set; }
        public int? StaffId { get; set; }
        public string? StaffName { get; set; }
        public List<string> Images { get; set; }
    }
}
