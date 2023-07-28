using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetListPaypal;
using Parking.FindingSlotManagement.Application;
using System.Net;
using Parking.FindingSlotManagement.Application.Features.Admin.Booking.BookingManagement.Queries.GetAllBookingForAdmin;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Route("api/admin/booking-management")]
    [ApiController]
    public class BookingManagementForAdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingManagementForAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        [HttpGet(Name = "GetAllBookingForAdmin")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllBookingForAdminQueryHandler>>>> GetAllBookingForAdmin([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetAllBookingForAdminQuery() { PageNo = pageNo, PageSize = pageSize };
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
