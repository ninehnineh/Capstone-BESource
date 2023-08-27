using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Commands.AcceptParkingRequest;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Commands.DeclineParkingRequest;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetAllParkingRequest;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetFieldInforByParkingId;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetListParkingWaitingToAccept;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetParkingInformationTab;
using Parking.FindingSlotManagement.Infrastructure.Hubs;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Route("api/approve-parkings")]
    [ApiController]
    public class ApproveParkingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessageHub> _messageHub;

        public ApproveParkingController(IMediator mediator, IHubContext<MessageHub> messageHub)
        {
            _mediator = mediator;
            _messageHub = messageHub;
        }
        /// <summary>
        /// API For Admin
        /// </summary>
        [HttpGet("all-field-information/{parkingId}", Name = "GetFieldInforByParkingId")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetFieldInforByParkingIdResponse>>> GetFieldInforByParkingId (int parkingId)
        {
            try
            {
                var query = new GetFieldInforByParkingIdQuery()
                {
                   ParkingId = parkingId
                };
                var res = await _mediator.Send(query);
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
        [HttpGet("request", Name = "GetAllParkingRequest")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetAllParkingRequestResponse>>> GetAllParkingRequest([FromQuery]int PageNo, [FromQuery]int PageSize)
        {
            try
            {
                var query = new GetAllParkingRequestQuery()
                {
                    PageNo = PageNo,
                    PageSize = PageSize
                };
                var res = await _mediator.Send(query);
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
        [HttpGet("request/waiting-accept", Name = "GetListParkingWaitingToAccept")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetListParkingWaitingToAcceptResponse>>> GetListParkingWaitingToAccept([FromQuery] int PageNo, [FromQuery] int PageSize)
        {
            try
            {
                var query = new GetListParkingWaitingToAcceptQuery()
                {
                    PageNo = PageNo,
                    PageSize = PageSize
                };
                var res = await _mediator.Send(query);
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
        [HttpGet("parking-information-tab/{parkingId}", Name = "GetParkingInformationTab")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetParkingInformationTabResponse>>> GetParkingInformationTab(int parkingId)
        {
            try
            {
                var query = new GetParkingInformationTabQuery()
                {
                    ParkingId = parkingId
                };
                var res = await _mediator.Send(query);
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
        [HttpPut("request/accept", Name = "AcceptParkingRequest")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> AcceptParkingRequest(AcceptParkingRequestCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadParkingInAdmin");
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
        [HttpPut("request/decline", Name = "DeclineParkingRequest")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceResponse<string>>> DeclineParkingRequest(DeclineParkingRequestCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);
                if (res.Message != "Thành công")
                {
                    return StatusCode((int)res.StatusCode, res);
                }
                await _messageHub.Clients.All.SendAsync("LoadParkingInAdmin");
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
