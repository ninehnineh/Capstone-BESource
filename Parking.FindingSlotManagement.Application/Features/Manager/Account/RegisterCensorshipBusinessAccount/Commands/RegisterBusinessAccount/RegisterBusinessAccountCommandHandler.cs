using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
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
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public RegisterBusinessAccountCommandHandler(IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository)
        {
            _userRepository = userRepository;
            _businessProfileRepository = businessProfileRepository;
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
                userEntity.IsCensorship = false;
                userEntity.RoleId = 1;
                await _userRepository.Insert(userEntity);
                businessProfileEntity.UserId = userEntity.UserId;
                await _businessProfileRepository.Insert(businessProfileEntity);
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

                throw new Exception(ex.Message);
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
