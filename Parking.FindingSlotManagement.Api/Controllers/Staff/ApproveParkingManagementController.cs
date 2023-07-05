using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.UpdateApproveParking;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Staff
{
    [Route("api/request/approve-parkings")]
    [ApiController]
    public class ApproveParkingManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApproveParkingManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Staff
        /// </summary>
        [HttpPut("parking", Name = "UpdateApproveParking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewApproveParking([FromBody] UpdateApproveParkingCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
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
