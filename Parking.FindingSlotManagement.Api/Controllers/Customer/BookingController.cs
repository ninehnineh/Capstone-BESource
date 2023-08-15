using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetPaypalByManagerId;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CaculateTotalPriceAfterSelectSlot;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CancelBooking;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.ChangeStatusToAlreadyPaid;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBooking;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBookingWhenAlreadyPaid;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PaymentMethod;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PostPaidOnline;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PrepaidLate;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PrePaidOnline;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetAvailableSlot;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetBookingDetails;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetCustomerActivities;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetListBookingFollowCalendar;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetUpcommingBooking;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckIn;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Customer
{
    [Route("api/customer-booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _hubContext;

        public BookingController(IMediator mediator, 
            IHubContext<MessageHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        /// 
        [Authorize(Roles = "Customer")]
        [HttpGet("upcomming/{userId}", Name = "GetUpcommingBookingByUserId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUpcommingBookingResponse>>>> GetUpcommingBookingByUserId(int userId)
        {
            try
            {
                var query = new GetUpcommingBookingQuery() { UserId = userId };
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
        /// API For Customer
        /// </summary>
        /// 
        [Authorize(Roles = "Customer")]
        [HttpGet("activities/{userId}", Name = "GetCustomerActivitiesByUserId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetCustomerActivitiesResponse>>>> GetCustomerActivitiesByUserId(int userId)
        {
            try
            {
                var query = new GetCustomerActivitiesQuery() { UserId = userId };
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
        /// API For Customer
        /// </summary>
        /// <remark>
        /// SignalR: CustomerCreateBookingSuccess, LoadHistoryInManager
        /// </remark>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<int>>> CreateBooking([FromBody] CreateBookingCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _hubContext.Clients.All.SendAsync("CustomerCreateBookingSuccess");
                    await _hubContext.Clients.All.SendAsync("LoadHistoryInManager");
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
        /// API For Customer
        /// </summary>
        /// 
        /// <remark>
        /// SignalR: CustomerCreateBookingSuccess
        /// </remark>
        [HttpPost("booking/prepaid-online-booking/already-paid")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateBookingwhenAlreadyPaid([FromBody] CreateBookingWhenAlreadyPaidCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _hubContext.Clients.All.SendAsync("CustomerCreateBookingSuccess");
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
        /// API For Customer
        /// </summary>
        /// <remark>
        /// SignalR: LoadHistoryInManager
        /// </remark>
        [HttpPost("cancel-booking")]
        public async Task<ActionResult<ServiceResponse<string>>> CancelBooking([FromBody] CancelBookingCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _hubContext.Clients.All.SendAsync("CustomerCreateBookingSuccess");
                    await _hubContext.Clients.All.SendAsync("LoadHistoryInManager");
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        [HttpPost("prepaid-late-booking")]
        public async Task<ActionResult<ServiceResponse<string>>> PrepaidLateBooking([FromBody] PrepaidLateCommand command)
        {
            try
            {
                command.context = HttpContext;
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        [HttpPost("postpaid-online-booking")]
        public async Task<ActionResult<ServiceResponse<string>>> PostpaidOnlineBooking([FromBody] PostPaidOnlineCommand command)
        {
            try
            {
                command.context = HttpContext;
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        [HttpPut("change-status-paid")]
        public async Task<ActionResult<ServiceResponse<string>>> ChangeStatusToAlreadyPaid([FromBody] ChangeStatusToAlreadyPaidCommand command)
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
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        [HttpPost("prepaid-online-booking")]
        public async Task<ActionResult<ServiceResponse<string>>> PrepaidOnlineBooking([FromBody] PrePaidOnlineCommand command)
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
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("check-in")]
        public async Task<ActionResult<ServiceResponse<string>>> CheckIn([FromBody] CheckInCommand command)
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
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("get-available-slots")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<ParkingSlot>>>> Get([FromQuery] GetAvailableSlotsQuery query)
        {
            try
            {
                var res = await _mediator.Send(query);
                if (res.Message == "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("get-available-slots-calendar")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListBookingFollowCalendarResponse>>>> GetCalendar([FromQuery] GetListBookingFollowCalendarQuery query)
        {
            try
            {
                var res = await _mediator.Send(query);
                if (res.Message == "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("update-payment-method")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdatePaymentMethod([FromBody] PaymentMethodCommand command)
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


        [HttpGet("getbooked-booking-detail")]
        public async Task<ActionResult<ServiceResponse<GetBookingDetailsResponse>>> GetBookingDetails(
            [FromQuery] GetBookingDetailsQuery query)
        {
            try
            {
                var res = await _mediator.Send(query);
                if (res.Message == "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("get-expected-price")]
        public async Task<ActionResult<ServiceResponse<int>>> GetExpectedPrice([FromQuery] CaculateTotalPriceAfterSelectSlotCommand query)
        {
            try
            {
                var res = await _mediator.Send(query);
                if (res.Message == "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
