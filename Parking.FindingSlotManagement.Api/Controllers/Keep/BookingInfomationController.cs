using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.BookingInformation;

namespace Parking.FindingSlotManagement.Api.Controllers.Keep
{
    [Route("api/keeper/booking-Infomation")]
    [ApiController]
    public class BookingInfomationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingInfomationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<BookingInformationResponse>>> BookingInformation([FromQuery] BookingInformationCommand command)
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
