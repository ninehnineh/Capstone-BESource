using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.CreateNewParkingSpotImage;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.DeleteParkingSpotImage;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.UpdateParkingSpotImage;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Queries.GetListImageByParkingId;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Route("api/parking-spot-image")]
    [ApiController]
    public class ParkingSpotImageController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public ParkingSpotImageController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingSpotImage
        /// </remarks>
        [Authorize(Roles = "Manager")]
        [HttpPost(Name = "CreateNewParkingSpotImage")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewParkingSpotImage([FromBody] CreateNewParkingSpotImageCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadParkingSpotImage");
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
        /// SignalR: LoadParkingSpotImage
        /// </remarks>
        [Authorize(Roles = "Manager")]
        [HttpDelete("{parkingSpotImageId}", Name = "DeleteParkingSpotImage")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteParkingSpotImage(int parkingSpotImageId)
        {
            try
            {
                var command = new DeleteParkingSpotImageCommand() { ParkingSpotImageId = parkingSpotImageId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadParkingSpotImage");
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
        /// SignalR: LoadParkingSpotImage
        /// </remarks>
        [Authorize(Roles = "Manager")]
        [HttpPut("{parkingSpotImageId}", Name = "UpdateParkingSpotImage")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateParkingSpotImage(int parkingSpotImageId, [FromBody] UpdateParkingSpotImageCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadParkingSpotImage");
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
        /// <summary>
        /// API For Manager, Customer, Keeper
        /// </summary>
        [Authorize(Roles = "Manager,Customer,Keeper,Admin")]
        [HttpGet("{parkingId}", Name = "GetListParkingSpotImage")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListImageByParkingIdResponse>>>> GetListParkingSpotImage(int parkingId, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetListImageByParkingIdQuery()
                {
                    PageNo = pageNo,
                    PageSize = pageSize,
                    ParkingId = parkingId
                };
                var res = await _mediator.Send(query);
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
