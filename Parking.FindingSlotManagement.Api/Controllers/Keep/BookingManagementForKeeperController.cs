using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetPaypalByManagerId;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllBookingByKeeperId;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Keep
{
    [Route("api/booking-management-for-keeper")]
    [ApiController]
    public class BookingManagementForKeeperController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingManagementForKeeperController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Keeper
        /// </summary>
        /// <param name="parkingId"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        /// 
        /*[Authorize(Roles = "Keeper")]*/
        [HttpGet("keeper/{keeperId}", Name = "SearchRequestBooking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<SearchRequestBookingResponse>>>> SearchRequestBooking(int keeperId, [FromQuery]string searchString)
        {
            try
            {
                var query = new SearchRequestBookingQuery() { KeeperId = keeperId, SearchString = searchString };
                var res = await _mediator.Send(query);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Keeper
        /// </summary>
        /// 
        /*[Authorize(Roles = "Keeper")]*/
        [HttpGet("{keeperId}/parkings", Name = "GetAllBookingByKeeperId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllBookingByKeeperIdResponse>>>> GetAllBookingByKeeperId(int keeperId, [FromQuery]int pageNo, [FromQuery]int pageSize)
        {
            try
            {
                var query = new GetAllBookingByKeeperIdQuery() { KeeperId = keeperId, PageNo = pageNo, PageSize = pageSize };
                var res = await _mediator.Send(query);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
