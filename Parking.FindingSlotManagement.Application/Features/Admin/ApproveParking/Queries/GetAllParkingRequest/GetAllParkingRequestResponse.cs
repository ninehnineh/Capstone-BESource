using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetAllParkingRequest
{
    public class GetAllParkingRequestResponse
    {
        public int ParkingId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? CarSpot { get; set; }
        public string BusinessProfileName { get; set; }
        public int BusinessId { get; set; }
        public string ApproveParkingStatus { get; set; }
    }
}
