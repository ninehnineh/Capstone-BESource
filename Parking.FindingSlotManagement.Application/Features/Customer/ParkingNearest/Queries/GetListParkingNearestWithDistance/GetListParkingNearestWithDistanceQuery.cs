using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestWithDistance
{
    public class GetListParkingNearestWithDistanceQuery : IRequest<ServiceResponse<IEnumerable<ParkingWithDistanceVer2>>>
    {
        public double CurrentLatitude { get; set; }
        public double CurrentLongtitude { get; set; }
        public int Distance { get; set; }
    }
}
