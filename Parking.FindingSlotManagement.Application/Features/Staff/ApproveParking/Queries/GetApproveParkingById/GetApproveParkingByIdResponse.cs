using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingById
{
    public class GetApproveParkingByIdResponse
    {
        public int? StaffId { get; set; }
        public string? StaffName { get; set; }
        public int ApproveParkingId { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public List<ImagesOfRequestApprove> Images { get; set; }
    }
    public class ImagesOfRequestApprove
    {
        public int FieldWorkParkingImgId { get; set; }
        public string? Url { get; set; }
    }
}
