using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Commands.DisableOrEnableAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetCustomerById;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetListCustomer;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public AccountController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpGet("customer", Name = "GetListCustomer")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListCustomerResponse>>>> GetListBusinessProfile([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetListCustomerQuery()
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
        /// 
        [Authorize(Roles = "Admin")]
        [HttpGet("customer/{userId}", Name = "GetCustomerByUserId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetCustomerByIdResponse>>> GetCustomerByUserId(int userId)
        {
            try
            {
                var query = new GetCustomerByIdQuery()
                {
                    UserId = userId
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
        /// SignalR: LoadCustomerList
        /// </remarks>
        [HttpPut("{userId}", Name = "DisableOrEnableAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DisableOrEnableAccount(int userId)
        {
            try
            {
                var command = new DisableOrEnableAccountCommand() { UserId = userId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadCustomerList");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
