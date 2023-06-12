using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Staff.Accounts.StaffAccountManagement.Commands.UpdatePasswordByStaffId;

namespace Parking.FindingSlotManagement.Api.Controllers.Staff
{
    [Authorize(Roles = "Staff")]
    [Route("api/my-staff-account")]
    [ApiController]
    public class StaffAccountManagementForStaffController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StaffAccountManagementForStaffController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Staff
        /// </summary>
        [HttpPut("{userId}", Name = "UpdatePasswordByStaffId")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdatePasswordByStaffId(int userId, UpdatePasswordByStaffIdCommand command)
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
