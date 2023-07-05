using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Account.RegisterCensorshipBusinessAccount.Commands.RegisterBusinessAccount;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Route("api/business-manager-authentication")]
    [ApiController]
    public class BusinessManagerAuthenticationController : ControllerBase
    {
        private readonly IBusinessManagerAuthenticationRepository _businessManagerAuthenticationRepository;
        private readonly IHubContext<MessageHub> _messageHub;
        private readonly IMediator _mediator;

        public BusinessManagerAuthenticationController(
            IBusinessManagerAuthenticationRepository businessManagerAuthenticationRepository, IHubContext<MessageHub> messageHub, IMediator mediator)
        {
            _businessManagerAuthenticationRepository = businessManagerAuthenticationRepository;
            _messageHub = messageHub;
            _mediator = mediator;
        }

        /// <summary>
        /// API for Manager
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<AuthResponse>>> ManagerLogin(AuthRequest request)
        {
            try
            {
                var manager = await _businessManagerAuthenticationRepository.ManagerLogin(request);
                return StatusCode((int)manager.StatusCode, manager);
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
        /// SignalR: LoadRequestRegisterCensorshipManagerAccounts
        /// </remarks>
        /// 
        [HttpPost("register", Name = "CreateNewBusinessManagerAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewBusinessManagerAccount([FromBody] RegisterBusinessAccountCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadRequestRegisterCensorshipManagerAccounts");
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
    }
}
