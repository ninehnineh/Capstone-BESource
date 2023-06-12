using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Customer.RatingParking.RatingManagement.Commands.UpdateRatingStarsOfParking;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Customer
{
    [Authorize(Roles = "Customer")]
    [Route("api/rating-stars")]
    [ApiController]
    public class RatingManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RatingManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        [HttpPut(Name = "UpdateRating")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateRatingStars([FromBody] UpdateRatingStarsOfParkingCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
/*                await _messageHub.Clients.All.SendAsync("LoadFavoriteAddressInCustomer");*/
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
