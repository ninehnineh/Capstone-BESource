using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.DeletePaypal;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;
using Parking.FindingSlotManagement.Application.Features.Staff.FieldWorkParkingImg.Commands.DeleteImagesById;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.CreateNewApproveParking;
using Parking.FindingSlotManagement.Application.Features.Staff.FieldWorkParkingImg.Commands.CreateNewImage;

namespace Parking.FindingSlotManagement.Api.Controllers.Staff
{
    [Route("api/field-work-parking-img")]
    [ApiController]
    public class FieldWorkParkingImgController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FieldWorkParkingImgController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Staff
        /// </summary>
        [HttpDelete("{fieldWorkParkingImgId}", Name = "DeleteImagesById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteImagesById(int fieldWorkParkingImgId)
        {
            try
            {
                var command = new DeleteImagesByIdCommand() { FieldWorkParkingImgId = fieldWorkParkingImgId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Staff
        /// </summary>
        [HttpPost(Name = "CreateNewImage")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewImage([FromBody] CreateNewImageCommand command)
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
