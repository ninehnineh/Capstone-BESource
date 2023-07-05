using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Queries.CustomerLogin
{
    public class CustomerLoginQueryHandler : IRequestHandler<CustomerLoginQuery, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        public CustomerLoginQueryHandler(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<ServiceResponse<string>> Handle(CustomerLoginQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<User, object>>> includes = new List<Expression<Func<User, object>>>
                {
                    x => x.Role
                };
                var checkAccountExist = await _userRepository.GetItemWithCondition(x => x.Phone.Equals(request.Phone), includes);
                if(checkAccountExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Số điện thoại không tồn tại. Vui lòng tạo tài khoản!!!",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if(checkAccountExist.IsActive == false)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Số điện thoại đã bị hệ thống ban, do quy phạm các nguyên tắc của hệ thống.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                if (checkAccountExist.RoleId == 3 && checkAccountExist.IsActive == true)
                {
                    TokenManage token = new TokenManage(_jwtSettings, _configuration);

                    return new ServiceResponse<string>
                    {
                        Success = true,
                        Message = "Thành công",
                        StatusCode = 200,
                        Data = token.GenerateToken(checkAccountExist).ToString()!,
                    };
                }
                else
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Xuất hiện lỗi, không thể đăng nhập vào hệ thống",
                        StatusCode = 404,
                        Success = false,
                    };
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
