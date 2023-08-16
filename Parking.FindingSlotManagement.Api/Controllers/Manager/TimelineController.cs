using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.CreateNewTimeline;
using Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.DisableOrEnableTimeline;
using Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.UpdateTimeline;
using Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Queries.GetListTimelineByParkingPriceId;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    
    [Route("api/timeline-management")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public TimelineController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        /// 
        [Authorize(Roles = "Manager,Admin,Staff")]
        [HttpGet("{parkingPriceId}", Name = "GetListTimelineByParkingPriceId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListTimelineByParkingPriceIdResponse>>>> GetListTimelineByParkingPriceId(int parkingPriceId, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetListTimelineByParkingPriceIdQuery()
                {
                    PageNo = pageNo,
                    PageSize = pageSize,
                    ParkingPriceId = parkingPriceId
                };
                var res = await _mediator.Send(query);
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadTimelineInManager
        /// </remarks>
        /// 
        [Authorize(Roles = "Manager")]
        [HttpPost(Name = "CreateNewTimeline")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewTimeline([FromBody] CreateNewTimelineCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadTimelineInManager");
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                IEnumerable<string> list1 = new List<string> { "Severity: Error" };
                string message = "";
                foreach (var item in list1)
                {
                    message = ex.Message.Replace(item, string.Empty);
                }
                var errorResponse = new ErrorResponseModel(ResponseCode.BadRequest, "Validation Error: " + message.Remove(0, 31));
                return StatusCode((int)ResponseCode.BadRequest, errorResponse);
            }
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadTimelineInManager
        /// </remarks>
        /// 
        [Authorize(Roles = "Manager")]
        [HttpDelete("{timelineId}", Name = "DisableOrEnableTimeline")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DisableOrEnableTimeline(int timelineId)
        {
            try
            {
                var command = new DisableOrEnableTimelineCommand() { TimeLineId = timelineId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadTimelineInManager");
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadTimelineInManager
        /// </remarks>
        /// 
        [Authorize(Roles = "Manager")]
        [HttpPut("{timelineId}", Name = "UpdateTimeline")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateTimeline(int timelineId, [FromBody] UpdateTimelineCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadTimelineInManager");
                return NoContent();
            }
            catch (Exception ex)
            {
                IEnumerable<string> list1 = new List<string> { "Severity: Error" };
                string message = "";
                foreach (var item in list1)
                {
                    message = ex.Message.Replace(item, string.Empty);
                }
                var errorResponse = new ErrorResponseModel(ResponseCode.BadRequest, "Validation Error: " + message.Remove(0, 31));
                return StatusCode((int)ResponseCode.BadRequest, errorResponse);
            }
        }
    }
}
