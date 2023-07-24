using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetListParkingWaitingToAccept;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.CreateNewApproveParking;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.SendRequestApproveParking;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.UpdateApproveParking;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingById;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingWithIntial;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListApproveParkingByParkingId;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListParkingNewWNoApprove;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Staff
{
    
    [Route("api/request/approve-parkings")]
    [ApiController]
    public class ApproveParkingManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public ApproveParkingManagementController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Staff
        /// </summary>
        /// <remarks>
        /// SignalR: LoadApproveParkingList
        /// </remarks>
        /// 
        [Authorize(Roles = "Staff")]
        [HttpPut("parking", Name = "UpdateApproveParking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateApproveParking([FromBody] UpdateApproveParkingCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadApproveParkingList");
                    return NoContent();
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
        /// API For Staff
        /// </summary>
        /// <remarks>
        /// SignalR: LoadApproveParkingList
        /// </remarks>
        /// 
        [Authorize(Roles = "Staff")]
        [HttpPost("parking", Name = "CreateNewApproveParking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewApproveParking([FromBody] CreateNewApproveParkingCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadApproveParkingList");
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
        /// API For Staff
        /// </summary>
        [Authorize(Roles = "Staff")]
        [HttpPut("parking/send-request/{approveParkingId}", Name = "SendRequestApproveParking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> SendRequestApproveParking(int approveParkingId)
        {
            try
            {
                var command = new SendRequestApproveParkingCommand
                {
                    ApproveParkingId = approveParkingId
                };
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadApproveParkingList");
                    return NoContent();
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
        /// API For Staff
        /// </summary>
        /// 
        [Authorize(Roles = "Staff")]
        [HttpGet("parking-profile/{parkingId}", Name = "GetListApproveParkingByParkingId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetListApproveParkingByParkingIdRes>>> GetListApproveParkingByParkingId(int parkingId)
        {
            try
            {
                var query = new GetListApproveParkingByParkingIdQuery { ParkingId = parkingId };
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
        /// API For Staff, Admin
        /// </summary>
        /// 
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("approve-parking-detail/{approveParkingId}", Name = "GetApproveParkingById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetApproveParkingByIdResponse>>> GetApproveParkingById(int approveParkingId)
        {
            try
            {
                var query = new GetApproveParkingByIdQuery { ApproveParkingId = approveParkingId };
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
        /// API For Staff, Admin
        /// </summary>
        /// 
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("approve-parking/initial/{parkingId}", Name = "GetApproveParkingWithIntial")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetApproveParkingWithIntialResponse>>> GetApproveParkingWithIntial(int parkingId)
        {
            try
            {
                var query = new GetApproveParkingWithIntialQuery { ParkingId = parkingId };
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
        /// API For Staff,Admin
        /// </summary>
        /// 
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("new-parkings/do-not-have-approve", Name = "GetListParkingNewWNoApprove")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListParkingNewWNoApproveResponse>>>> GetListParkingNewWNoApprove([FromQuery] int PageNo, [FromQuery] int PageSize)
        {
            try
            {
                var query = new GetListParkingNewWNoApproveQuery()
                {
                    PageNo = PageNo,
                    PageSize = PageSize
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
