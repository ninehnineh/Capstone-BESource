using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.DisableOrEnableTimeline
{
    public class DisableOrEnableTimelineCommand : IRequest<ServiceResponse<string>>
    {
        public int TimeLineId { get; set; }
    }
}
