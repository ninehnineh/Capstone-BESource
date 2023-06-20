using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetParkingDetails;
using Parking.FindingSlotManagement.Infrastructure.Hubs;

namespace Parking.FindingSlotManagement.Api.Controllers.Customer
{
    [Route("api/parkings")]
    [ApiController]
    public class ParkingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParkingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("mobile/parking-details")]
        public async Task<ActionResult<ServiceResponse<GetParkingDetailsResponse>>> GetParkingDetails(
            [FromQuery] GetParkingDetailsQuery query)
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
