using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.UpdatePaypal;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.ChangeSlotForCustomer;
using Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetAvailableSlotByFloorId;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.GetAvailableSlotByFloorIdForKeeper;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.ChangeSlotWhenComeEarly;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAvailableSlotByFloorIdVer2;
using Parking.FindingSlotManagement.Application.Features.Keeper.ParkingSlots.Commands.DisableParkingSlot;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.DisableParkingByDate;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.EnableParkingSlot;

namespace Parking.FindingSlotManagement.Api.Controllers.Keep
{
    [Route("api/keeper/parking-slot")]
    [ApiController]
    public class ParkingSlotController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParkingSlotController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Keeper
        /// </summary>
        [HttpPut("change", Name = "ChangeSlotForCustomer")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> ChangeSlotForCustomer([FromBody] ChangeSlotForCustomerCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                IEnumerable<string> list1 = new List<string> { "Severity: Error" };
                string message = "";
                foreach (var item in list1)
                {
                    message = ex.Message.Replace(item, string.Empty);
                }
                var errorResponse = new ErrorResponseModel(ResponseCode.BadRequest, "Validation Error: " + message.Remove(0, 31));
                return StatusCode((int)ResponseCode.BadRequest, errorResponse);
            }
        }
        /// <summary>
        /// API For Keeper
        /// </summary>
        [HttpPut("change/come-early", Name = "ChangeSlotWhenComeEarly")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> ChangeSlotWhenComeEarly([FromBody] ChangeSlotWhenComeEarlyCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                IEnumerable<string> list1 = new List<string> { "Severity: Error" };
                string message = "";
                foreach (var item in list1)
                {
                    message = ex.Message.Replace(item, string.Empty);
                }
                var errorResponse = new ErrorResponseModel(ResponseCode.BadRequest, "Validation Error: " + message.Remove(0, 31));
                return StatusCode((int)ResponseCode.BadRequest, errorResponse);
            }
        }
        /// <summary>
        /// API For Keeper
        /// </summary>
        [HttpGet("floors/floor/parking-slots")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAvailableSlotByFloorIdResponse>>>> GetAvailableParkingSlots
            ([FromQuery] GetAvailableSlotByFloorIdForKeeperQuery query)
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
        /// <summary>
        /// API For Keeper
        /// </summary>
        [HttpGet("floors/floor/parking-slots/ver2/passerby")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAvailableSlotByFloorIdResponse>>>> GetAvailableParkingSlotsForPasswerBy
            ([FromQuery] GetAvailableSlotByFloorIdVer2Query query)
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

        [HttpPost("disable")]
        public async Task<ActionResult<ServiceResponse<string>>> Disable([FromBody] DisableParkingSlotCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at Disable slot: " + ex.Message);
            }
        }

        [HttpPut("enable")]
        public async Task<ActionResult<ServiceResponse<string>>> Enable( [FromBody] EnableParkingSlotCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return NoContent();
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Error at Enable slot: " + ex.Message);
            }
        }
    }
}
