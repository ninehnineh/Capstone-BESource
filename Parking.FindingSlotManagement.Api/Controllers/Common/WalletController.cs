using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Common.WalletManagement.Commands.DepositToAWallet;
using Parking.FindingSlotManagement.Application.Models.Wallet;
using System.Net;

namespace Parking.FindingSlotManagement.Api.Controllers.Common
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("deposit", Name = "DepositToAWallet")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DepositToAWallet([FromBody] DepositToAWalletCommand command)
        {
            command.Context = HttpContext;
            var res = await _mediator.Send(command);
            if (res.Message == "Thành công")
            {
                return StatusCode((int)res.StatusCode, res);
            }
            return StatusCode((int)res.StatusCode, res);
        }
        [HttpPost("deposit/manager", Name = "DepositToAWalletManager")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DepositToAWallet([FromBody] DepositToAWalletManagerCommand command)
        {
            command.Context = HttpContext;
            var res = await _mediator.Send(command);
            if (res.Message == "Thành công")
            {
                return StatusCode((int)res.StatusCode, res);
            }
            return StatusCode((int)res.StatusCode, res);
        }
    }
}
