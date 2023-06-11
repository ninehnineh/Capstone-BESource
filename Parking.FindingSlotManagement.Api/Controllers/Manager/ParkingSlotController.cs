using MediatR;
using MediatR.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.Create;
using Parking.FindingSlotManagement.Infrastructure.Hubs;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Authorize(Roles = "Manager")]
    [Route("api/parkingSlot")]
    [ApiController]
    public class ParkingSlotController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _hubContext;

        public ParkingSlotController(IMediator mediator,
            IHubContext<MessageHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }


        /// <summary>
        /// Api for Manager
        /// </summary>
        /// <remark>SignalR: LoadParkingSlot</remark>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<int>>> Create([FromBody] CreateParkingSlotsCommand command)
        {
            try
            {
                var respose = await _mediator.Send(command);
                if (respose.Message == "Thành công")
                {
                    await _hubContext.Clients.All.SendAsync("LoadParkingSlot");
                    return StatusCode((int)respose.StatusCode, respose);
                }
                return StatusCode((int)respose.StatusCode, respose);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
