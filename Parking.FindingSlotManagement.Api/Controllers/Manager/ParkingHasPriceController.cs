using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.CreateParkingHasPrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetListParkingHasPriceWithPagination;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetParkingHasPriceDetailWithPagination;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [ApiController]
    [Route("api/parkingHasPrice")]
    public class ParkingHasPriceController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IHubContext<MessageHub> hubContext;
        
        public ParkingHasPriceController(IMediator mediator,
            IHubContext<MessageHub> hubContext)
        {
            this.hubContext = hubContext;
            this.mediator = mediator;
        }

        /*/// <summary>
        /// API for Manager
        /// get all parking has price of specific manager
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("getlistparkinghasprice")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListParkingHasPriceWithPaginationResponse>>>> GetListParkingHasPrice([FromQuery]GetListParkingHasPriceWithPaginationQuery request)
        {
            try
            {
                var query = new GetListParkingHasPriceWithPaginationQuery
                {
                    ManagerId = request.ManagerId, 
                    PageNo = request.PageNo, 
                    PageSize = request.PageSize
                };               
                var response = await mediator.Send(query);

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
                var response = await mediator.Send(query);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
*/
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
                var res = await mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await hubContext.Clients.All.SendAsync("LoadParkingHasPrice");
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

    }
}