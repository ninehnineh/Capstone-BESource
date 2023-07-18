using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingById;
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
        public string Status { get; set; }
        /*        public List<ImagesOfRequestApprove> Images { get; set; }*/
    }
}
