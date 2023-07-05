using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Commands.CustomerRegister;
using Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Queries.CustomerLogin;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Customer
{
    [Route("api/mobile/customer-authentication")]
    [ApiController]
    public class AuthenticationCustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationCustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API for Customer
        /// </summary>
        [HttpPost("login", Name ="CustomerLogin")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<string>>> CustomerLogin(CustomerLoginQuery request)
        {
            try
            {
                var res = await _mediator.Send(request);
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
        /// API for Customer
        /// </summary>
        [HttpPost("register", Name = "CustomerRegister")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<string>>> CustomerRegister(CustomerRegisterCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
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
