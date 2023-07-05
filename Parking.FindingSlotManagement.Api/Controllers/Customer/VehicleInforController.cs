using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Commands.CreateNewVehicleInfo;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Commands.DeleteVehicleInfor;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Commands.UpdateVehicleInfo;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Queries.GetListVehicleInforByUserId;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Queries.GetVehicleInforById;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Customer
{
    [Authorize(Roles = "Customer")]
    [Route("api/vehicle-infor")]
    [ApiController]
    public class VehicleInforController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public VehicleInforController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        /// <remarks>
        /// SignalR: LoadVehicleInforInCustomer
        /// </remarks>
        [HttpPost(Name = "CreateNewVehicleInfor")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewVehicleInfor([FromBody] VehicleInfoCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadVehicleInforInCustomer");
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
        /// SignalR: LoadVehicleInforInCustomer
        /// </remarks>
        [HttpPut("{vehicleInforId}", Name = "UpdateVehicleInfor")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateVehicleInfor(int vehicleInforId, [FromBody] UpdateVehicleInfoCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadVehicleInforInCustomer");
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
         /// <remarks>
         /// SignalR: LoadVehicleInforInCustomer
         /// </remarks>
        [HttpDelete("{vehicleInforId}", Name = "DeleteVehicleInfor")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteVehicleInfor(int vehicleInforId)
        {
            try
            {
                var command = new DeleteVehicleInforCommand() { VehicleInforId = vehicleInforId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadVehicleInforInCustomer");
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
        [HttpGet("{vehicleInforId}", Name = "GetVehicleInforById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetVehicleInforByIdResponse>>> GetVehicleInforById(int vehicleInforId)
        {
            try
            {
                var query = new GetVehicleInforByIdQuery() { VehicleInforId = vehicleInforId };
                var res = await _mediator.Send(query);

                return StatusCode((int)res.StatusCode, res);

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        [HttpGet("/user/{userId}/vehicle-infor", Name = "GetListVehicleInforByUserId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListVehicleInforByUserIdResponse>>>> GetListVehicleInforByUserId(int userId, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetListVehicleInforByUserIdQuery() { UserId = userId, PageNo = pageNo, PageSize = pageSize };
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
