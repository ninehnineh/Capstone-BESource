using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetListParkingDesByRating;
using Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestWithDistance;
using Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestYou;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Customer
{
    [Authorize(Roles = "Customer")]
    [Route("api/parking-nearest")]
    [ApiController]
    public class ParkingNearestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParkingNearestController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Customer, Guest
        /// </summary>
        [AllowAnonymous]
        [HttpGet(Name = "GetListParkingNearestYou")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<ParkingWithDistance>>>> GetListParkingNearestYou([FromQuery] double currentLatitude,[FromQuery] double currentLongtitude)
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
        /// <summary>
        /// API For Customer, Guest
        /// </summary>
        [AllowAnonymous]
        [HttpGet("distance", Name = "GetListParkingNearestWithDistance")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<ParkingWithDistanceVer2>>>> GetListParkingNearestWithDistance([FromQuery] double currentLatitude, [FromQuery] double currentLongtitude, [FromQuery] int distance)
        {
            try
            {
                var query = new GetListParkingNearestWithDistanceQuery() { CurrentLatitude = currentLatitude, CurrentLongtitude = currentLongtitude, Distance = distance };
                var res = await _mediator.Send(query);

                return StatusCode((int)res.StatusCode, res);

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Customer, Guest
        /// </summary>
        [AllowAnonymous]
        [HttpGet("/api/parkings-for-cus/ratings", Name = "GetListParkingDesByRating")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListParkingDesByRatingResponse>>>> GetListParkingDesByRating([FromQuery] int PageNo, [FromQuery] int PageSize)
        {
            try
            {
                var query = new GetListParkingDesByRatingQuery() { PageNo = PageNo, PageSize = PageSize };
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
