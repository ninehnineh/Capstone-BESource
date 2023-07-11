using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.AutoCancel.commands.AutomationCancelBooking;

namespace Parking.FindingSlotManagement.Api.Controllers.AutoCancel
{
    [Route("api/systems")]
    [ApiController]
    public class SystemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SystemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{bookingId}")]
        public async Task<ActionResult<ServiceResponse<string>>> AutoChangeBookingStatusToCancel
            (int bookingId)
        {
            try
            {
                var command = new AutomationCancelBookingCommand { BookingId = bookingId};
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
