using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Features.Customer.Wallet.Queries.GetWalletByUserId;
using Parking.FindingSlotManagement.Application;
using System.Net;
using Parking.FindingSlotManagement.Application.Features.Admin.Wallet.Queries.GetWalletForAdmin;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Route("api/admin-wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API For Customer
        /// </summary>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpGet(Name = "GetWalletOfAdmin")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceResponse<GetWalletForAdminResponse>>> GetWalletOfAdmin()
        {
            try
            {
                var query = new GetWalletForAdminQuery();
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
