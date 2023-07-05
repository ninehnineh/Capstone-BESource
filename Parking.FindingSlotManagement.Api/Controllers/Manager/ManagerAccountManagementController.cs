using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.Account.ManagerAccountManagement.Commands.UpdatePasswordByManagerId;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Authorize(Roles = "Manager,Keeper")]
    [Route("api/my-manager-account")]
    [ApiController]
    public class ManagerAccountManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManagerAccountManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        [HttpPut("{managerId}", Name = "UpdatePasswordByManagerId")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdatePasswordByManagerId(int managerId, UpdatePasswordByManagerIdCommand command)
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
