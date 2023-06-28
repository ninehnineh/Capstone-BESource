using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.CreateNewCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.DeleteCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.UpdateCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Queries.GetCensorshipManagerAccountList;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.NonCensorshipManagerAccount.Queries.GetNonCensorshipManagerAccountList;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Commands.AcceptRequestRegisterAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Commands.DeclineRequestRegisterAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Queries;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    
    [Route("api/managers")]
    [ApiController]
    public class ManagerManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public ManagerManagementController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Admin
        /// </summary> 
        [Authorize(Roles = "Admin")]
        [HttpGet("censorship", Name = "GetCensorshipManagerAccountList")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<CensorshipManagerAccountResponse>>>> GetCensorshipManagerAccountList([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetCensorshipManagerAccountListQuery()
                {
                    PageNo = pageNo,
                    PageSize = pageSize
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
        /// API For Admin
        /// </summary>
        /// <remarks>
        /// SignalR: LoadCensorshipManagerAccounts
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpDelete("censorship/{managerId}", Name = "DisableOrEnableCensorshipManagerAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DisableOrEnableCensorshipManagerAccount(int managerId)
        {
            try
            {
                var command = new DisableOrEnableManagerAccountCommand() { UserId = managerId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadCensorshipManagerAccounts");
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Admin, Manager, Keeper
        /// </summary>
        /// <remarks>
        /// SignalR: LoadCensorshipManagerAccounts
        /// </remarks>
        [Authorize(Roles = "Admin,Manager,Keeper")]
        [HttpPut("censorship/{managerId}", Name = "UpdateCensorshipManagerAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateCensorshipManagerAccount(int managerId, [FromBody] UpdateCensorshipManagerAccountCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadCensorshipManagerAccounts");
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
        /// SignalR: LoadCensorshipManagerAccounts
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPost("register/censorship", Name = "CreateNewCensorshipManagerAccountList")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewCensorshipManagerAccountList([FromBody] CreateNewCensorshipManagerAccountCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadCensorshipManagerAccounts");
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                IEnumerable<string> list1 = new List<string> {"Severity: Error"};
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
        [Authorize(Roles = "Admin")]
        [HttpGet("request/register-censorship", Name = "GetRequestRegisterCensorshipManagerAccountList")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<RequestResponse>>>> GetRequestRegisterCensorshipManagerAccountList([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetRequestAccountListQuery()
                {
                    PageNo = pageNo,
                    PageSize = pageSize
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
        /// API For Admin
        /// </summary>
        /// <remarks>
        /// SignalR: LoadRequestRegisterCensorshipManagerAccounts
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPut("request/register-censorship/accept/{userId}", Name = "AcceptRequestRegisterAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> AcceptRequestRegisterAccount(int userId)
        {
            try
            {
                var command = new AcceptRequestRegisterAccountCommand()
                {
                    UserId = userId
                };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadRequestRegisterCensorshipManagerAccounts");
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
        /// <remarks>
        /// SignalR: LoadRequestRegisterCensorshipManagerAccounts
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPut("request/register-censorship/decline/{userId}", Name = "DeclineRequestRegisterAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DeclineRequestRegisterAccount(int userId)
        {
            try
            {
                var command = new DeclineRequestRegisterAccountCommand()
                {
                    UserId = userId
                };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadRequestRegisterCensorshipManagerAccounts");
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
        [Authorize(Roles = "Admin")]
        [HttpGet("non-censorship", Name = "GetNonCensorshipManagerAccountList")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<NonCensorshipManagerAccountResponse>>>> GetNonCensorshipManagerAccountList([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetNonCensorshipManagerAccountListQuery()
                {
                    PageNo = pageNo,
                    PageSize = pageSize
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
