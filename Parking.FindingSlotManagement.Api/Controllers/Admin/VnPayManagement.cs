﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.CreateNewVnPay;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.DeleteVnPay;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.UpdateVnPay;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByManagerId;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Route("api/vnpay")]
    [ApiController]
    public class VnPayManagement : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public VnPayManagement(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        /// <remarks>
        /// SignalR: LoadVnPays
        /// </remarks>
        [HttpPost(Name = "CreateNewVnPay")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNewVnPay([FromBody] CreateNewVnPayCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message == "Thành công")
                {
                    await _messageHub.Clients.All.SendAsync("LoadVnPays");
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
        /// API For Admin
        /// </summary>
        /// <remarks>
        /// SignalR: LoadVnPays
        /// </remarks>
        [HttpPut("{vnPayId}", Name = "UpdateVnPay")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateVnPay(int vnPayId, [FromBody] UpdateVnPayCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadVnPays");
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
        /// API For Admin
        /// </summary>
        /// <remarks>
        /// SignalR: LoadVnPays
        /// </remarks>
        [HttpDelete("{vnPayId}", Name = "DeleteVnPay")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteVnPay(int vnPayId)
        {
            try
            {
                var command = new DeleteVnPayCommand() { VnPayId = vnPayId };
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadVnPays");
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
        [HttpGet("{managerId}", Name = "GetVnpayInforByManagerId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetVnPayByManagerIdResponse>>> GetVnpayInforByManagerId(int managerId)
        {
            try
            {
                var query = new GetVnPayByManagerIdQuery() { ManagerId = managerId };
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
