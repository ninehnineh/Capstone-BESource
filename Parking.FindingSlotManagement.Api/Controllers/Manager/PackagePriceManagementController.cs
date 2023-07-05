/*using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.CreateNewPackagePrice;
using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.DisableOrEnablePackagePrice;
using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.UpdatePackagePrice;
using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Queries.GetPackagePriceById;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Route("api/package-price")]
    [ApiController]
    public class PackagePriceManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public PackagePriceManagementController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadPackagePriceManagementInManager
        /// </remarks>
        [HttpPost(Name = "CreateNewPackagePrice")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewPackagePrice([FromBody] CreateNewPackagePriceCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadPackagePriceManagementInManager");
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
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadPackagePriceManagementInManager
        /// </remarks>
        [HttpDelete("{packagePriceId}", Name = "DisableOrEnablePackagePrice")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DisableOrEnablePackagePrice(int packagePriceId)
        {
            try
            {
                var command = new DisableOrEnablePackagePriceCommand() { PackagePriceId = packagePriceId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadPackagePriceManagementInManager");
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        /// <summary>
        /// API For Manager
        /// </summary>
        /// <remarks>
        /// SignalR: LoadPackagePriceManagementInManager
        /// </remarks>
        [HttpPut("{packagePriceId}", Name = "UpdatePackagePrice")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdatePackagePrice(int packagePriceId, [FromBody] UpdatePackagePriceCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadPackagePriceManagementInManager");
                return NoContent();
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
        /// <summary>
        /// API For Manager
        /// </summary>
        [HttpGet("{packagePriceId}", Name = "GetPackagePriceById")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetPackagePriceByIdResponse>>> GetPackagePriceById(int packagePriceId)
        {
            try
            {
                var query = new GetPackagePriceByIdQuery()
                {
                    PackagePriceId = packagePriceId
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
*/