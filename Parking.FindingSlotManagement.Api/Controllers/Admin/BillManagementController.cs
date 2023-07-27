using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Features.Admin.Bill.BillManagement.Queries.GetAllBills;
using Parking.FindingSlotManagement.Application;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Route("api/bill-management")]
    [ApiController]
    public class BillManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BillManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        [HttpGet(Name = "GetAllBills")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllBillsResponse>>>> GetAllBills([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetAllBillsQuery() { PageNo = pageNo, PageSize = pageSize };
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
