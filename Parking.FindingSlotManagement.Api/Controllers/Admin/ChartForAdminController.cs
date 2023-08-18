using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCardByParkingId;
using Parking.FindingSlotManagement.Application;
using System.Net;
using Parking.FindingSlotManagement.Application.Features.Admin.Chart.SumOfBusinessAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Chart.SumOfCustomer;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetRevenueByParkingId;
using Parking.FindingSlotManagement.Application.Features.Admin.Chart.SumOfFeeFromBusiness;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Route("api/admin/chart")]
    [ApiController]
    public class ChartForAdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChartForAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        [HttpGet("business/total", Name = "SumOfBusinessAccount")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<int>>> SumOfBusinessAccount()
        {
            try
            {
                var query = new SumOfBusinessAccountQuery();
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
        /// <summary>
        /// API For Admin
        /// </summary>
        [HttpGet("customer/total", Name = "SumOfCustomer")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<int>>> SumOfCustomer()
        {
            try
            {
                var query = new SumOfCustomerQuery();
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

        /// <summary>
        /// API For Manager
        /// </summary>
        [HttpGet("line/bill/month-revenue", Name = "SumOfFeeFromBusiness")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<SumOfFeeFromBusinessResponse>>>> SumOfFeeFromBusiness()
        {
            try
            {
                var query = new SumOfFeeFromBusinessQuery();
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
