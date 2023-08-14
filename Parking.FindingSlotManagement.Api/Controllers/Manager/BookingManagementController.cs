using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetListPaypal;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.ApproveBooking;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckOut;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.Done;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetAllBookingByParkingId;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetBookingById;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetListBookingByManagerId;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Route("api/booking-management")]
    [ApiController]
    public class BookingManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _hubContext;

        public BookingManagementController(IMediator mediator,
             IHubContext<MessageHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        [HttpGet("request/{managerId}", Name = "GetListBookingByManagerId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>>> GetListBookingByManagerId([FromQuery] int pageNo, [FromQuery] int pageSize, int managerId)
        {
            try
            {
                var query = new GetListBookingByManagerIdQuery()
                {
                    PageNo = pageNo,
                    PageSize = pageSize,
                    ManagerId = managerId
                };
                var res = await _mediator.Send(query);
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
        [HttpGet("{bookingId}", Name = "GetBookingById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetBookingByIdResponse>>> GetBookingById(int bookingId)
        {
            try
            {
                var query = new GetBookingByIdQuery { BookingId = bookingId };
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
        /// API for Manager
        /// </summary>
        [HttpPost("approve-booking")]
        public async Task<ActionResult<ServiceResponse<string>>> ApproveBooking([FromBody] ApproveBookingCommand command)
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
                //IEnumerable<string> list1 = new List<string> { "Severity: Error" };
                //string message = "";
                //foreach (var item in list1)
                //{
                //    message = ex.Message.Replace(item, string.Empty);
                //}
                //var errorResponse = new ErrorResponseModel(ResponseCode.BadRequest, "Validation Error: " + message.Remove(0, 31));
                //return StatusCode((int)ResponseCode.BadRequest, errorResponse);
                throw new Exception(ex.Message);
            }
        }
        /// <remarks>
        /// SignalR: LoadHistoryInManager
        /// </remarks>
        [HttpPut("check-out")]
        public async Task<ActionResult<ServiceResponse<string>>> CheckOut([FromBody] CheckOutCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _hubContext.Clients.All.SendAsync("LoadHistoryInManager");
                    return NoContent();
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("done")]
        public async Task<ActionResult<ServiceResponse<string>>> Done([FromBody] DoneCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    return NoContent();
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        [HttpGet("parkings/{parkingId}", Name = "GetAllBookingByParkingId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllBookingByParkingIdResponse>>>> GetAllBookingByParkingId(int parkingId, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetAllBookingByParkingIdQuery() { ParkingId = parkingId, PageNo = pageNo, PageSize = pageSize };
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
