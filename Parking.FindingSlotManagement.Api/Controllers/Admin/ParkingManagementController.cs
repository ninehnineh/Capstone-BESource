using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.ParkingManagement.Queries.GetAllParkingForAdmin;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Route("api/admin/parking-management")]
    [ApiController]
    public class ParkingManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParkingManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpGet(Name = "GetAllParkingForAdmin")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllParkingForAdminResponse>>>> GetAllParkingForAdmin([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetAllParkingForAdminQuery() { PageNo = pageNo, PageSize = pageSize };
                var res = await _mediator.Send(query);

                return StatusCode((int)res.StatusCode, res);

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
