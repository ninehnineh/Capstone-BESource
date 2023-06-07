using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestYou;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingNearestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParkingNearestController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        [HttpGet(Name = "GetListParkingNearestYou")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<ParkingWithDistance>>>> GetFavoriteAddressById([FromQuery] double currentLatitude,[FromQuery] double currentLongtitude)
        {
            try
            {
                var query = new GetListParkingNearestYouQuery() { CurrentLatitude = currentLatitude, CurrentLongtitude = currentLongtitude};
                var res = await _mediator.Send(query);

                return StatusCode((int)res.StatusCode, res);

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
