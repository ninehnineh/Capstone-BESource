using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetNumberOfDoneAndCancelBooking;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetNumberOfDoneAndCancelBookingByParkingId;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetRevenueByParkingId;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetRevenueManagerId;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCard;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCardByParkingId;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Route("api/chart")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChartController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        [HttpGet("pie/done-cancel-booking", Name = "GetNumberOfDoneAndCancelBooking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetNumberOfDoneAndCancelBookingRes>>> GetNumberOfDoneAndCancelBooking(int managerId)
        {
            try
            {
                var query = new GetNumberOfDoneAndCancelBookingQuery() { ManagerId = managerId };
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
        /// API For Manager
        /// </summary>
        [HttpGet("pie/parkings/{parkingId}/done-cancel-booking", Name = "GetNumberOfDoneAndCancelBookingByParkingId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetNumberOfDoneAndCancelBookingByParkingIdRes>>> GetNumberOfDoneAndCancelBookingByParkingId(int parkingId)
        {
            try
            {
                var query = new GetNumberOfDoneAndCancelBookingByParkingIdQuery() { ParkingId = parkingId };
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
        /// API For Manager
        /// </summary>
        [HttpGet("line/month-or-week-revenue", Name = "GetRevenueManagerId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetRevenueManagerIdResponse>>>> GetNumberOfDoneAndCancelBooking(int managerId, [FromQuery]string? week, [FromQuery]string? month)
        {
            try
            {
                var query = new GetRevenueManagerIdQuery() { ManagerId = managerId,  Week = week, Month = month};
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
        /// API For Manager
        /// </summary>
        [HttpGet("line/parkings/{parkingId}/month-or-week-revenue", Name = "GetRevenueByParkingId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetRevenueByParkingIdResponse>>>> GetRevenueByParkingId(int parkingId, [FromQuery] string? week, [FromQuery] string? month)
        {
            try
            {
                var query = new GetRevenueByParkingIdQuery() { ParkingId = parkingId, Week = week, Month = month };
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
        /// API For Manager
        /// </summary>
        [HttpGet("card/statistic-card", Name = "GetStatisticBaseOnCardManagerId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetStatisticBaseOnCardResponse>>> GetStatisticBaseOnCardManagerId(int managerId)
        {
            try
            {
                var query = new GetStatisticBaseOnCardQuery() { ManagerId = managerId};
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
        /// API For Manager
        /// </summary>
        [HttpGet("card/parkings/{parkingId}/statistic-card", Name = "GetStatisticBaseOnCardByParkingId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetStatisticBaseOnCardByParkingIdResponse>>> GetStatisticBaseOnCardByParkingId(int parkingId)
        {
            try
            {
                var query = new GetStatisticBaseOnCardByParkingIdQuery() { ParkingId = parkingId };
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
