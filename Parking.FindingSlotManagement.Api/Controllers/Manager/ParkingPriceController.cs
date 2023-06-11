using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloor;
using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.DisableOrEnablePackagePrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.CreateParkingPrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.DisableOrEnableParkingPrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Queries.GetAllParkingPrice;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Drawing.Printing;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Authorize(Roles = "Manager")]
    [Route("api/parking-price")]
    [ApiController]
    public class ParkingPriceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _hubContext;

        public ParkingPriceController(IMediator mediator,
            IHubContext<MessageHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }
        
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingPrice
        /// </remarks>
        [HttpPost("create")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateParkingPrice([FromBody] CreateParkingPriceCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _hubContext.Clients.All.SendAsync("LoadParkingPrice");
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
                var errorResponse = new ErrorResponseModel(ResponseCode.BadRequest, "Validation Error: " + message.Remove(0, 25));
                return StatusCode((int)ResponseCode.BadRequest, errorResponse);
            }
        }

        /// <summary>
        /// Api for Manager to disable or enable parking price
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingPrice
        /// </remarks>
        [HttpPut("disable-or-enable-parking-price")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DisableOrEnableParkingPrice([FromBody] DisableOrEnableParkingPriceCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _hubContext.Clients.All.SendAsync("LoadParkingPrice");
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
                var errorResponse = new ErrorResponseModel(ResponseCode.BadRequest, "Validation Error: " + message.Remove(0, 25));
                return StatusCode((int)ResponseCode.BadRequest, errorResponse);
            }
        }
        /*/// <summary>
        /// Api for Manager to modify the parking price
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingPrice
        /// </remarks>
        [HttpPut("update-parking-price")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateParkingPrice([FromBody] UpdateParkingPriceCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _hubContext.Clients.All.SendAsync("LoadParkingPrice");
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
                var errorResponse = new ErrorResponseModel(ResponseCode.BadRequest, "Validation Error: " + message.Remove(0, 25));
                return StatusCode((int)ResponseCode.BadRequest, errorResponse);
            }
        }
*/

        /// <summary>
        /// Api for Manager
        /// </summary>
        /// <param name="request"></param>
        /// <returns>List Parking Price</returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<GetAllParkingPriceQueryResponse>>> GetAllParkingPrice([FromQuery] GetAllParkingPriceQuery request)
        {
            try
            {
                var res = await _mediator.Send(request);
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
