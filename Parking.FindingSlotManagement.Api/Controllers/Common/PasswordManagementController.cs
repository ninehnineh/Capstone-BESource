using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Common.PasswordManagement.Commands.ForgotPassword;

namespace Parking.FindingSlotManagement.Api.Controllers.Common
{
    [Route("api/password-management")]
    [ApiController]
    public class PasswordManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PasswordManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        [HttpPut("forgot-password", Name = "ForgotPassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword(ForgotPasswordCommand command)
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
