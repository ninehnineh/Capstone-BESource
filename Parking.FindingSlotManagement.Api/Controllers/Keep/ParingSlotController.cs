using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.UpdatePaypal;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.ChangeSlotForCustomer;

namespace Parking.FindingSlotManagement.Api.Controllers.Keep
{
    [Route("api/keeper/parking-slot")]
    [ApiController]
    public class ParingSlotController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParingSlotController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Keeper
        /// </summary>
        [HttpPut("change", Name = "ChangeSlotForCustomer")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> ChangeSlotForCustomer([FromBody] ChangeSlotForCustomerCommand command)
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
