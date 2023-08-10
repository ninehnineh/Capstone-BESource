using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.DisableOrEnableStaffAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.UpdatePasswordForStaff;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.UpdateStaffAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Queries.GetListStaffAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Queries.GetStaffAccountById;
using Parking.FindingSlotManagement.Application.Features.Manager.Account.StaffAccountManagement.Commands.CreateNewStaffAccount;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    
    [Route("api/staff-account-management")]
    [ApiController]
    public class StaffAccountManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public StaffAccountManagementController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        /// <remarks>
        /// SignalR: LoadStaffAccounts
        /// </remarks>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpPost("register", Name = "CreateNewStaffAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewStaffAccount([FromBody] CreateNewStaffAccountCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadStaffAccounts");
                    return StatusCode(res.StatusCode, res);
                }
                return StatusCode(res.StatusCode, res);
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
        /// API For Admin
        /// </summary>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpGet(Name = "GetListStaffAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListStaffAccountResponse>>>> GetListStaffAccount([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetListStaffAccountQuery { PageNo = pageNo, PageSize = pageSize };
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
        /// API For Admin
        /// </summary>
        /// <remarks>
        /// SignalR: LoadStaffAccounts
        /// </remarks>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpPut(Name = "UpdateStaffAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateStaffAccount([FromBody] UpdateStaffAccountCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadStaffAccounts");
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
        /// API For Admin
        /// </summary>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpPut("password",Name = "UpdateStaffAccountPassword")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateStaffAccountPassword([FromBody] UpdatePasswordForStaffCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
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
        /// API For Admin
        /// </summary>
        /// <remarks>
        /// SignalR: LoadStaffAccounts
        /// </remarks>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpDelete("{staffId}", Name = "DisableOrEnableStaffAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DisableOrEnableStaffAccount(int staffId)
        {
            try
            {
                var command = new DisableOrEnableStaffAccountCommand() { UserId = staffId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadStaffAccounts");
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        /// 
        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("{staffId}", Name = "GetStaffAccountById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetStaffAccountByIdResponse>>> GetStaffAccountById(int staffId)
        {
            try
            {
                var query = new GetStaffAccountByIdQuery()
                {
                    UserId = staffId
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
