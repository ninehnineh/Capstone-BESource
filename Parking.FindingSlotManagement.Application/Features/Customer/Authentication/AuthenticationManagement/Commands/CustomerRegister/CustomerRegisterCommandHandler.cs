using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Queries.CustomerLogin;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Commands.CustomerRegister
{
    public class CustomerRegisterCommandHandler : IRequestHandler<CustomerRegisterCommand, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        private readonly JwtSettings _jwtSettings;
        public CustomerRegisterCommandHandler(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<ServiceResponse<string>> Handle(CustomerRegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _userRepository.GetItemWithCondition(x => x.Phone.Equals(request.Phone));
                if(checkUserExist != null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Số điện thoại đã được đăng kí.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var _mapper = config.CreateMapper();
                var entity = _mapper.Map<User>(request);
                entity.IsActive = true;
                entity.RoleId = 3;
                await _userRepository.Insert(entity);
                List<Expression<Func<User, object>>> includes = new List<Expression<Func<User, object>>>
                {
                    x => x.Role
                };
                var checkAccountExist = await _userRepository.GetItemWithCondition(x => x.UserId == entity.UserId, includes);
                TokenManage token = new TokenManage(_jwtSettings);

                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = "Thành công",
                    StatusCode = 200,
                    Data = token.GenerateToken(checkAccountExist).ToString()!,
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
