using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetListKeeperByManagerId;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Route("api/keeper-management")]
    [ApiController]
    public class KeeperManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KeeperManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        [HttpGet("manager", Name = "GetListKeeperByManagerId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>>> GetListKeeperByManagerId([FromQuery] int pageNo, [FromQuery] int pageSize, [FromQuery] int managerId)
        {
            try
            {
                var query = new GetListKeeperByManagerIdQuery()
                {
                    PageNo = pageNo,
                    PageSize = pageSize,
                    ManagerId = managerId
                };
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
