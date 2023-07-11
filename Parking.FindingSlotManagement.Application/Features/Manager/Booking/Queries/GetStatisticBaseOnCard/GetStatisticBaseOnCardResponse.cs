using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCard
{
    public class GetStatisticBaseOnCardResponse
    {
        public int NumberOfOrders { get; set; }
        public decimal TotalOfRevenue { get; set; }
        public int NumberOfOrdersInCurrentDay { get; set; }
        public int WaitingOrder { get; set; }
    }
}
