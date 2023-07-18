using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetPaypalByManagerId;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBooking;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.CreateBookingForPasserby;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.FilterBookingForKeeper;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllBookingByKeeperId;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Keep
{
    [Route("api/booking-management-for-keeper")]
    [ApiController]
    public class BookingManagementForKeeperController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public BookingManagementForKeeperController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Keeper
        /// </summary>
        /// <remark>
        /// SignalR: KeeperCreateBookingForPasserby
        /// </remark>
        [HttpPost("create/passerby", Name = "CreateBookingForPasserby")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateBookingForPasserby([FromBody] CreateBookingForPasserbyCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("KeeperCreateBookingForPasserby");
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
                //return StatusCode(500, ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// API For Keeper
        /// </summary>
        /// <param name="parkingId"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Keeper")]
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
        [Authorize(Roles = "Keeper")]
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
        /// <summary>
        /// API For Keeper
        /// </summary>
        /// 
        [Authorize(Roles = "Keeper")]
        [HttpGet("filters/{keeperId}/parkings", Name = "FilterBookingForKeeper")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllBookingByKeeperIdResponse>>>> FilterBookingForKeeper(int keeperId, [FromQuery]DateTime? date, [FromQuery]string? status, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new FilterBookingForKeeperQuery() { KeeperId = keeperId, Date = date, Status = status, PageNo = pageNo, PageSize = pageSize };
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
