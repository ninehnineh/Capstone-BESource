using AutoMapper;
using Hangfire;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Account.RegisterCensorshipBusinessAccount.Commands.RegisterBusinessAccount
{
    public class RegisterBusinessAccountCommandHandler : IRequestHandler<RegisterBusinessAccountCommand, ServiceResponse<int>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        private readonly IBillRepository _billRepository;
        private readonly IFeeRepository _feeRepository;
        private readonly IWalletRepository _walletRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public RegisterBusinessAccountCommandHandler(IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository,
            IBillRepository billRepository, IFeeRepository feeRepository, IWalletRepository walletRepository)
        {
            _userRepository = userRepository;
            _businessProfileRepository = businessProfileRepository;
            _billRepository = billRepository;
            _feeRepository = feeRepository;
            _walletRepository = walletRepository;
        }
        public async Task<ServiceResponse<int>> Handle(RegisterBusinessAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _mapper = config.CreateMapper();
                var userEntity = _mapper.Map<User>(request.UserEntity);
                var businessProfileEntity = _mapper.Map<Domain.Entities.BusinessProfile>(request.BusinessProfileEntity);
                CreatePasswordHash(request.UserEntity.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);
                userEntity.PasswordHash = passwordHash;
                userEntity.PasswordSalt = passwordSalt;
                userEntity.IsActive = true;
                userEntity.IsCensorship = true;
                userEntity.RoleId = 1;
                await _userRepository.Insert(userEntity);
                var entityWallet = new Wallet
                {
                    Balance = 0.0M,
                    Debt = 0.0M,
                    UserId = userEntity.UserId
                };
                await _walletRepository.Insert(entityWallet);
                var feeExist = await _feeRepository.GetItemWithCondition(x => x.FeeId == businessProfileEntity.FeeId);
                var getUser = await _userRepository.GetItemWithCondition(x => x.UserId == userEntity.UserId);
                if (feeExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy phí.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(feeExist.FeeId == 1)
                {
                    businessProfileEntity.Type = "Tư nhân";
                }
                else if(feeExist.FeeId == 2)
                {
                    businessProfileEntity.Type = "Doanh nghiệp";
                }
                businessProfileEntity.UserId = userEntity.UserId;
                await _businessProfileRepository.Insert(businessProfileEntity);
                
                Bill entityBill = new()
                {
                    Time = DateTime.UtcNow.AddHours(7),
                    Status = BillStatus.Chờ_Thanh_Toán.ToString(),
                    Price = feeExist.Price,
                    BusinessId = businessProfileEntity.BusinessProfileId
                };
                await _billRepository.Insert(entityBill);
                var timeToCancel = DateTime.UtcNow.AddHours(7).AddDays(30);

                var a = BackgroundJob.Schedule<IServiceManagement>(
                    x => x.ChargeMoneyFor1MonthUsingSystem(feeExist, businessProfileEntity.BusinessProfileId, entityBill.BillId, getUser), timeToCancel);

                return new ServiceResponse<int>
                {
                    Data = userEntity.UserId,
                    Message = "Thành công",
                    StatusCode = 201,
                    Success = true
                };

            }
            catch (Exception ex)
            {

                throw new Exception("Handler Error:" + ex.Message);
            }

        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
