using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetListParkingHasPriceWithPagination;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetParkingHasPriceDetailWithPagination;
using Parking.FindingSlotManagement.Infrastructure.Hubs;

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

        /// <summary>
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

    }
}