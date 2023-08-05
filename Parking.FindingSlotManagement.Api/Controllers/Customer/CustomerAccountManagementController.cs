using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Commands.UpdateCustomerProfileById;
using Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Queries.GetBanCountByUserId;
using Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Queries.GetCustomerProfileById;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Customer
{
    [Authorize(Roles = "Customer")]
    [Route("api/mobile")]
    [ApiController]
    public class CustomerAccountManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerAccountManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        [HttpGet("account/{userId}", Name = "GetCustomerProfileById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetCustomerProfileByIdResponse>>> GetCustomerProfileById(int userId)
        {
            try
            {
                var query = new GetCustomerProfileByIdQuery() { UserId = userId };
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
        [HttpGet("account/ban-count/{userId}", Name = "GetBanCountByUserId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetBanCountByUserIdResponse>>> GetBanCountByUserId(int userId)
        {
            try
            {
                var query = new GetBanCountByUserIdQuery() { UserId = userId };
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
        [HttpPut("account/{userId}", Name = "UpdateCustomerProfileById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateCustomerProfileById(int userId, [FromBody] UpdateCustomerProfileByIdCommand command)
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
    }
}
