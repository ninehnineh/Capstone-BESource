using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetAvailableSlotByFloorId;

namespace Parking.FindingSlotManagement.Api.Controllers.Customer
{
    [Route("api/parking-slots")]
    [ApiController]
    public class ParkingSlotsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParkingSlotsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /*[HttpGet("parking-slots")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetParkingSlotsResponse>>>> GetAvailableParkingSlots
            ([FromQuery] GetParkingSlotsQuery query)
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
        }*/
        [HttpGet("floors/floorId/parking-slots")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAvailableSlotByFloorIdResponse>>>> GetAvailableParkingSlots
            ([FromQuery] GetAvailableSlotByFloorIdQuery query)
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
