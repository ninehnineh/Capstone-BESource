using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestYou
{
    public class GetListParkingNearestYouQuery : IRequest<ServiceResponse<IEnumerable<ParkingWithDistance>>>
    {
        public double CurrentLatitude { get; set; }
        public double CurrentLongtitude { get; set; }
    }
}
