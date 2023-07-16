using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.ParkingManagement.Commands.DisableOrEnableParking;
using Parking.FindingSlotManagement.Application.Features.Admin.ParkingManagement.Queries.GetAllParkingForAdmin;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Route("api/admin/parking-management")]
    [ApiController]
    public class ParkingManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public ParkingManagementController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpGet(Name = "GetAllParkingForAdmin")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllParkingForAdminResponse>>>> GetAllParkingForAdmin([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetAllParkingForAdminQuery() { PageNo = pageNo, PageSize = pageSize };
                var res = await _mediator.Send(query);

                return StatusCode((int)res.StatusCode, res);

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        /// <remark>
        /// SignalR: LoadParkingInAdmin
        /// </remark>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpDelete("{parkingId}", Name = "DisableOrEnableParkingForAdmin")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DisableOrEnableParkingForAdmin(int parkingId)
        {
            try
            {
                var command = new DisableOrEnableParkingCommand() { ParkingId = parkingId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadParkingInAdmin");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
