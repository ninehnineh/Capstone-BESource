﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetRevenueManagerId
{
    public class GetRevenueManagerIdResponse
    {
        public DateTime Date { get; set; }
        public decimal RevenueOfTheDate { get; set; }
    }
}
