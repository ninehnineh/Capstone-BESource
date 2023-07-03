using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Application.Models.Wallet;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.WalletManagement.Commands.DepositToAWallet
{
    public class DepositToAWalletCommandHandler : IRequestHandler<DepositToAWalletCommand, ServiceResponse<string>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IVnPayService _vnPayService;
        public DepositToAWalletCommandHandler(IWalletRepository walletRepository, IUserRepository userRepository, IConfiguration configuration, IVnPayService vnPayService)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _vnPayService = vnPayService;
        }
        public async Task<ServiceResponse<string>> Handle(DepositToAWalletCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string res = "";
                List<Expression<Func<User, object>>> includes = new List<Expression<Func<User, object>>>
                {
                    x => x.VnPays
                };
                var checkUserExist = await _userRepository.GetItemWithCondition(x => x.UserId == request.UserId && x.IsActive == true, includes, true);
                if(checkUserExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản hoặc tài khoản đã bị hệ thống ban.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                DepositTransaction dt = new DepositTransaction
                {
                    TotalPrice = request.TotalPrice,
                    UserId = request.UserId
                };
                if(checkUserExist.RoleId == 3)
                {
                    res = _vnPayService.CreatePaymentUrlForDeposit(dt, _configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], request.Context);
                }
                else if(checkUserExist.RoleId == 1)
                {
                    res = _vnPayService.CreatePaymentUrlForDeposit(dt, checkUserExist.VnPays.FirstOrDefault().TmnCode, checkUserExist.VnPays.FirstOrDefault().HashSecret, request.Context);
                }
                else
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Tài khoản của bạn không được phép nạp tiền vào ví.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                return new ServiceResponse<string>
                {
                    Data = res,
                    Success = true,
                    StatusCode = 201,
                    Message = "Thành công"
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
