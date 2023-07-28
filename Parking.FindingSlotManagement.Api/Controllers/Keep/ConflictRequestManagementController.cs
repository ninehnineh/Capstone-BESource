using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetListPaypal;
using Parking.FindingSlotManagement.Application;
using System.Net;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllConflictRequestByKeeperId;

namespace Parking.FindingSlotManagement.Api.Controllers.Keep
{
    [Route("api/conflict-request")]
    [ApiController]
    public class ConflictRequestManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConflictRequestManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Keeper
        /// </summary>
        [HttpGet("keeper/{keeperId}", Name = "GetAllConflictRequestByKeeperId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllConflictRequestByKeeperIdResponse>>>> GetAllConflictRequestByKeeperId(int keeperId, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetAllConflictRequestByKeeperIdQuery() { KeeperId = keeperId, PageNo = pageNo, PageSize = pageSize };
                var res = await _mediator.Send(query);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                return StatusCode((int)res.StatusCode, res);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
