using MediatR;
using MediatR.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.Create;
using Parking.FindingSlotManagement.Application.Features.Keeper.ParkingSlots.Commands.DisableParkingSlot;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.UpdateParkingSlots;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetListParkingSlotByFloorId;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.DisableParkingByDate;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{

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
        /// API For Manager
        /// </summary>
        /// 
        [HttpGet("floor/{floorId}", Name = "GetListParkingSlotByFloorId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetListParkingSlotByFloorIdResponse>>> GetListParkingSlotByFloorId(int floorId)
        {
            try
            {
                var query = new GetListParkingSlotByFloorIdQuery { FloorId = floorId };
                var res = await _mediator.Send(query);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Api for Manager
        /// </summary>
        /// <remark>SignalR: LoadParkingSlot</remark>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        //[Authorize(Roles = "Manager")]
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
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingSlot
        /// </remarks>
        /// 
        [Authorize(Roles = "Manager")]
        [HttpPut(Name = "UpdateParkingSlot")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateParkingSlot([FromBody] UpdateParkingSlotsCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _hubContext.Clients.All.SendAsync("LoadParkingSlot");
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
