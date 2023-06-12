using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Commands.DeleteBusinessProfile;
using Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetBusinessProfileById;
using Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetListBusinessProfile;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/business-profile-management")]
    [ApiController]
    public class BusinessProfileManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public BusinessProfileManagementController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }

        /// <summary>
        /// API For Admin
        /// </summary>
        [HttpGet("business-profile/{businessProfileId}", Name = "GetBusinessProfileById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetBusinessProfileByIdResponse>>> GetBusinessProfileById(int businessProfileId)
        {
            try
            {
                var query = new GetBusinessProfileByIdQuery { BusinessProfileId = businessProfileId };
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
        /// <remarks>
        /// SignalR: LoadBusinessProfileInAdmin
        /// </remarks>
        [HttpDelete("business-profile/{businessProfileId}", Name = "DeleteBusinessProfile")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteBusinessProfile(int businessProfileId)
        {
            try
            {
                var command = new DeleteBusinessProfileCommand() { BusinessProfileId = businessProfileId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadBusinessProfileInAdmin");
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        [HttpGet(Name = "GetListBusinessProfile")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetListBusinessProfileResponse>>>> GetListBusinessProfile([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var query = new GetListBusinessProfileQuery()
                {
                    PageNo = pageNo,
                    PageSize = pageSize
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
