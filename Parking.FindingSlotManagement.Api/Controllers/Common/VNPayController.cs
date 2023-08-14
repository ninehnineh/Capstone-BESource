using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Common.TransactionManagement.Commands.CreateNewTransaction;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Hubs;

namespace Parking.FindingSlotManagement.Api.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IWalletRepository _walletRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHubContext<MessageHub> _messageHub;

        public VNPayController(IVnPayService vnPayService, IWalletRepository walletRepository, IMediator mediator, IMapper mapper, ITransactionRepository transactionRepository, IHubContext<MessageHub> messageHub)
        {
            _vnPayService = vnPayService;
            _walletRepository = walletRepository;
            _mediator = mediator;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _messageHub = messageHub;
        }
        [HttpGet]
        public PaymentResponseModel GetTransactions()
        {
            var res = _vnPayService.PaymentExecute(Request.Query);
            return res;
        }
        [HttpGet("/api/VNPayDeposit")]
        public async Task<PaymentResponseModel> GetTransactionsDeposit([FromQuery] int userId)
        {
            var res = _vnPayService.PaymentExecuteForDeposit(Request.Query);
            if (res.VnPayResponseCode.Equals("00"))
            {

                Wallet entity = new Wallet
                {
                    Balance = decimal.Parse(res.OrderDescription),
                    UserId = userId
                };
                await _walletRepository.UpdateMoneyInWallet(entity, "00");
               return res;

            }
            else
            {

                Wallet entity = new Wallet
                {
                    Balance = 0M,
                    UserId = userId
                };
                await _walletRepository.UpdateMoneyInWallet(entity, null);
                return res;

            }
        }
        /// <remarks>
        /// SignalR: LoadHistoryInManager
        /// </remarks>
        [HttpGet("/api/VNPayDeposit/manager")]
        public async Task<IActionResult> GetTransactionsDepositManager([FromQuery] int userId)
        {
            var res = _vnPayService.PaymentExecuteForDeposit(Request.Query);
            if (res.VnPayResponseCode.Equals("00"))
            {

                Wallet entity = new Wallet
                {
                    Balance = decimal.Parse(res.OrderDescription),
                    UserId = userId
                };
                await _walletRepository.UpdateMoneyInWallet(entity, "00");
                await _messageHub.Clients.All.SendAsync("LoadHistoryInManager");
                return Redirect("https://park-z-manager-web.vercel.app/wallet");

            }
            else
            {

                Wallet entity = new Wallet
                {
                    Balance = 0M,
                    UserId = userId
                };
                await _walletRepository.UpdateMoneyInWallet(entity, null);
                await _messageHub.Clients.All.SendAsync("LoadHistoryInManager");
                return Redirect("https://park-z-manager-web.vercel.app/failed");

            }
        }
    }
}
