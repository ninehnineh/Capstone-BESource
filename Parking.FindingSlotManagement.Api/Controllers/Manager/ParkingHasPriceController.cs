using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.CreateParkingHasPrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.DeleteParkingHasPrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.DeleteParkingHasPriceVer2;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.UpdateParkingHasPrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetListParkingHasPriceWithPagination;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetParkingHasPriceDetailWithPagination;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Authorize(Roles = "Manager")]
    [ApiController]
    [Route("api/parkingHasPrice")]
    public class ParkingHasPriceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _hubContext;
        
        public ParkingHasPriceController(IMediator mediator,
            IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
            _mediator = mediator;
        }

        /// <summary>
        /// API for Manager
        /// get all parking has price of specific manager
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("getlistparkinghasprice")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>>> GetListParkingHasPrice([FromQuery] GetListParkingHasPriceWithPaginationQuery request)
        {
            try
            {
                var query = new GetListParkingHasPriceWithPaginationQuery
                {
                    ParkingId = request.ParkingId,
                    PageNo = request.PageNo,
                    PageSize = request.PageSize
                };
                var response = await _mediator.Send(query);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Api for Manager
        /// get one parking has price with parking has price Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<ServiceResponse<GetParkingHasPriceDetailWithPaginationResponse>>> GetDetailParkingHasPrice(int id)
        {
            try
            {
                var query = new GetParkingHasPriceDetailWithPaginationQuery { ParkingHasPriceId = id };
                var response = await _mediator.Send(query);

                return StatusCode((int)response.StatusCode, response);
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
        /// SignalR: LoadParkingHasPrice
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<int>>> CreateParkingHasPrice([FromBody] CreateParkingHasPriceCommand command )
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _hubContext.Clients.All.SendAsync("LoadParkingHasPrice");
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
        /// Api for Manager to delete the parking has price
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingHasPrice
        /// </remarks>
        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteParkingHasPrice(int id)
        {
            try
            {
                DeleteParkingHasPriceCommand command = new DeleteParkingHasPriceCommand { ParkingHasPriceId = id };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _hubContext.Clients.All.SendAsync("LoadParkingHasPrice");
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
        /// <summary>
        /// Api for Manager to delete the parking has price
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingHasPrice
        /// </remarks>
        [HttpDelete("v2/{parkingId}/{parkingPriceId}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteParkingHasPriceV2(int parkingId, int parkingPriceId)
        {
            try
            {
                var command = new DeleteParkingHasPriceVer2Command { ParkingId = parkingId, ParkingPriceId = parkingPriceId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _hubContext.Clients.All.SendAsync("LoadParkingHasPrice");
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
        /// <summary>
        /// Api for Manager to modify the parking has price
        /// </summary>
        /// <remarks>
        /// SignalR: LoadParkingHasPrice
        /// </remarks>
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateParkingHasPrice([FromBody] UpdateParkingHasPriceCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _hubContext.Clients.All.SendAsync("LoadParkingHasPrice");
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
    }
}