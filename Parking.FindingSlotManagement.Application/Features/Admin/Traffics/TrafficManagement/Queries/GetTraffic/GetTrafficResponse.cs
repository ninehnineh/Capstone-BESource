using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetTraffic
{
    public class GetTrafficResponse
    {
        public int TrafficId { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
