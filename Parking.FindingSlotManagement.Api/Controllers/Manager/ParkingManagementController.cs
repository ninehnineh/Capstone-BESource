using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.DisableParkingAtCurrentTime;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.EnableParking;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CancelDisableScheduledParking;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.ChangeStatusFull;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CreateNewParking;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.DisableOrEnableParking;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.UpdateLocationOfParking;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.UpdateParking;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByManagerId;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByParkingPriceId;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetParkingById;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.DisableParkingByDate;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetDisableParkingHistory;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetSuccessedDisableParkingHistory;
using Parking.FindingSlotManagement.Application.Models.Parking;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{

    [Route("api/parkings")]
    [ApiController]
    public class ParkingManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public ParkingManagementController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingInAdmin
        /// </remarks>
        /// 
        [Authorize(Roles = "Manager")]
        [HttpPost("parking", Name = "CreateNewParking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewParking([FromBody] CreateNewParkingCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadParkingInAdmin");
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
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
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingInAdmin
        /// </remarks>
        /// 
        [Authorize(Roles = "Manager")]
        [HttpPut("parking/location/{parkingId}", Name = "UpdateLocationOfParking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateLocationOfParking(int parkingId, [FromBody] UpdateLocationCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadParkingInAdmin");
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
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingInAdmin
        /// </remarks>
        /// 
        [Authorize(Roles = "Manager")]
        [HttpDelete("parking/{parkingId}", Name = "DisableOrEnableParking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DisableOrEnableParking(int parkingId)
        {
            try
            {
                var command = new DisableOrEnableParkingCommand() { ParkingId = parkingId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadParkingInAdmin");
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingInAdmin
        /// </remarks>
        /// 
        [Authorize(Roles = "Manager")]
        [HttpPut("parking/{parkingId}", Name = "UpdateParking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateParking(int parkingId, [FromBody] UpdateParkingCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadParkingInAdmin");
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
        /// API For Manager, Keeper
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingInAdmin
        /// </remarks>
        /// 
        [Authorize(Roles = "Manager")]
        [Authorize(Roles = "Manager,Keeper")]
        [HttpPut("parking/full/{parkingId}", Name = "UpdateStatusFullOfParking")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateStatusFullOfParking(int parkingId)
        {
            try
            {
                var command = new ChangeStatusFullCommand
                {
                    ParkingId = parkingId
                };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadParkingInAdmin");
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
        /// API For Manager
        /// </summary>
        /// 
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet(Name = "GetListParkingByManagerId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListParkingByManagerIdResponse>>>> GetListParkingByManagerId(int managerId, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetListParkingByManagerIdQuery() { ManagerId = managerId, PageNo = pageNo, PageSize = pageSize };
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
        /// 
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet("{parkingId}", Name = "GetParkingById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetParkingByIdResponse>>> GetParkingById(int parkingId)
        {
            try
            {
                var query = new GetParkingByIdQuery() { ParkingId = parkingId };
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
        /// 
        [Authorize(Roles = "Manager")]
        [HttpGet("parking-price/{parkingPriceId}", Name = "GetParkingByParkingPriceId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListParkingByParkingPriceIdResponse>>>> GetParkingByParkingPriceId(int parkingPriceId)
        {
            try
            {
                var query = new GetListParkingByParkingPriceIdQuery() { ParkingPriceId = parkingPriceId };
                var res = await _mediator.Send(query);

                return StatusCode((int)res.StatusCode, res);

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("scheduled-history-disable-parking")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Job>>>> GetDisableParking(int parkingId)
        {
            try
            {
                var query = new GetDisableParkingHistoryQuery() { ParkingId = parkingId };
                var res = await _mediator.Send(query);

                return StatusCode((int)res.StatusCode, res);

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("successed-history-disable-parking")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Job>>>> GetSuccessedDisableParking(int parkingId)
        {
            try
            {
                var query = new GetSuccessedDisableParkingHistoryQuery() { ParkingId = parkingId };
                var res = await _mediator.Send(query);

                return StatusCode((int)res.StatusCode, res);

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("enable-disable-parking-at-date")]
        public async Task<ActionResult<ServiceResponse<string>>> EnableDisableParking([FromBody] EnableParkingAtDateCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("disable-parking-by-date")]
        public async Task<ActionResult<ServiceResponse<string>>> DisableByDate([FromBody] DisableParkingByDateCommand command)
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
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("cancel-disable-scheduled-parking")]
        public async Task<ActionResult<ServiceResponse<string>>> CancelDisableScheduledParking([FromBody] CancelDisableScheduledParkingCommand command)
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
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("disable-parking-by-date-time")]
        public async Task<ActionResult<ServiceResponse<string>>> DisableByDateTime([FromBody] DisableParkingAtCurrentTimeCommand command)
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
                throw new Exception(ex.Message);
            }
        }
    }
}
