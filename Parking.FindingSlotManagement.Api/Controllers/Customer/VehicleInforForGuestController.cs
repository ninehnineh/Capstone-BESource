using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Commands.CreateVehicleInfoForGuest;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Commands.DeleteVehicleInfoForGuest;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Commands.UpdateVehicleInfoForGuest;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Queries.GetVehicleInfoForGuestById;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Customer
{
    [Authorize(Roles = "Customer")]
    [Route("api/vehicle-infor-guest")]
    [ApiController]
    public class VehicleInforForGuestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public VehicleInforForGuestController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        /// <remarks>
        /// SignalR: LoadVehicleInforForGuestInCustomer
        /// </remarks>
        [HttpPost(Name = "CreateNewVehicleInforForGuest")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewVehicleInforForGuest([FromBody] VehicleInfoForGuestCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadVehicleInforForGuestInCustomer");
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
        /// API For Customer
        /// </summary>
        /// <remarks>
        /// SignalR: LoadVehicleInforForGuestInCustomer
        /// </remarks>
        [HttpDelete("{vehicleInforId}", Name = "DeleteVehicleInfoForGuest")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteVehicleInfoForGuest(int vehicleInforId)
        {
            try
            {
                var command = new DeleteVehicleInforForGuestCommand() { VehicleInforId = vehicleInforId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadVehicleInforForGuestInCustomer");
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        /// <remarks>
        /// SignalR: LoadVehicleInforForGuestInCustomer
        /// </remarks>
        [HttpPut("{vehicleInforId}", Name = "UpdateVehicleInfoForGuest")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateVehicleInfoForGuest(int vehicleInforId, [FromBody] UpdateVehicleInfoForGuestCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadVehicleInforForGuestInCustomer");
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
        /// API For Customer
        /// </summary>
        [HttpGet("{vehicleInforId}", Name = "GetVehicleInfoForGuestById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetVehicleInfoForGuestByIdResponse>>> GetVehicleInfoForGuestById(int vehicleInforId)
        {
            try
            {
                var query = new GetVehicleInfoForGuestByIdQuery() { VehicleInforId = vehicleInforId };
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
