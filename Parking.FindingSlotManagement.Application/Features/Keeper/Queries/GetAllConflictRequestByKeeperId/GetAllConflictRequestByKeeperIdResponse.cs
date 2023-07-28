using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllConflictRequestByKeeperId
{
    public class GetAllConflictRequestByKeeperIdResponse
    {
        public int ConflictRequestId { get; set; }
        public int ParkingId { get; set; }
        public string Message { get; set; }
        public int BookingId { get; set; }
        public string Status { get; set; }
    }
}
