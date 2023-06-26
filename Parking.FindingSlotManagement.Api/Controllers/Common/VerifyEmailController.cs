using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Common.VerifyEmail.Queries.VerifyEmailExist;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Common
{
    [Route("api/verify-email")]
    [ApiController]
    public class VerifyEmailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VerifyEmailController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        [HttpGet(Name = "VerifyEmail")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<string>>> VerifyEmail([FromQuery] string email)
        {
            try
            {
                var query = new VerifyEmailExistQuery() { Email = email };
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
