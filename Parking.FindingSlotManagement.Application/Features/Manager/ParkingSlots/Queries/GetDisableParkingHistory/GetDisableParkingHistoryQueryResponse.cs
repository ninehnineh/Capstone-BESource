using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetDisableParkingHistory
{
    public class GetDisableParkingHistoryQueryResponse
    {
        public int ParkingId { get; set; }
        public string State { get; set; }
        public string DisableDate { get; set; }
        public string CreatedAt { get; set; }
        public string Reason { get; set; }
    }
}