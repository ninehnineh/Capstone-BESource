﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CancelBooking;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBooking;
using Parking.FindingSlotManagement.Infrastructure.Hubs;

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

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<int>>> CreateBooking([FromBody] CreateBookingCommand command)
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

        [HttpPost("cancel-booking")]
        public async Task<ActionResult<ServiceResponse<string>>> CancelBooking([FromBody] CancelBookingCommand command)
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
    }
}