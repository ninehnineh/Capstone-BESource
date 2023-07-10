using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCardByParkingId
{
    public class GetStatisticBaseOnCardByParkingIdResponse
    {
        public int NumberOfOrders { get; set; }
        public decimal TotalOfRevenue { get; set; }
        public int NumberOfOrdersInCurrentDay { get; set; }
        public int WaitingOrder { get; set; }
    }
}
