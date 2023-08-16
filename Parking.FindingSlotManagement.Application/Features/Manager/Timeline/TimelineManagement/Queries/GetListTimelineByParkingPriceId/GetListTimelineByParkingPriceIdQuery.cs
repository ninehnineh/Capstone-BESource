using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Queries.GetListTimelineByParkingPriceId
{
    public class GetListTimelineByParkingPriceIdQuery : IRequest<ServiceResponse<GetListTimelineByParkingPriceIdResponse>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int ParkingPriceId { get; set; }
    }
}
