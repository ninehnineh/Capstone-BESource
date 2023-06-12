using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories.AuthenticationRepositories
{
    public class StaffAuthenticationRepository : IStaffAuthenticationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ParkZDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public StaffAuthenticationRepository(IConfiguration configuration,
            IOptions<JwtSettings> jwtSettings,
            IHttpContextAccessor httpContextAccessor,
            ParkZDbContext DbContext)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = DbContext;
            _jwtSettings = jwtSettings.Value;
        }


        public async Task<ServiceResponse<AuthResponse>> StaffLogin(AuthRequest request)
        {
            var response = new ServiceResponse<AuthResponse>();

            if (string.IsNullOrEmpty(request.Email.Trim()))
            {
                response.Success = false;
                response.Message = "Vui lòng nhập email";
                response.StatusCode = 404;

                return response;
            }

            if (string.IsNullOrEmpty(request.Password.Trim()))
            {
                response.Success = false;
                response.Message = "Vui lòng nhập password";
                response.StatusCode = 404;

                return response;
            }

            var user = await _dbContext.Users.Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email!.Equals(request.Email));

            if (user == null)
            {
                response.Success = false;
                response.Message = "Người dùng không tồn tại, đăng nhập thất bại";
                response.StatusCode = 404;

                return response;
            }
            else if (!PasswordManage.VerifyPasswordHash(request.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                response.Success = false;
                response.Message = "Sai mật khẩu";
                response.StatusCode = 200;

                return response;
            }

            if (user!.IsActive == false)
            {
                return new ServiceResponse<AuthResponse>
                {
                    Message = "Tài khoản đã bị khóa",
                    StatusCode = 200,
                    Success = false,
                };
            }

            if (user.Role!.Name!.Equals("Staff") && user!.IsActive == true)
            {
                TokenManage token = new TokenManage(_jwtSettings, _configuration);

                response.Success = true;
                response.Message = $"Chào mừng {user.Name}";
                response.StatusCode = 200;
                response.Data = new AuthResponse
                {
                    Token = token.GenerateToken(user).ToString()!,
                };

                return response;
            }
            else
            {
                return new ServiceResponse<AuthResponse>
                {
                    Message = "Xuất hiện lỗi, không thể đăng nhập vào hệ thống",
                    StatusCode = 404,
                    Success = false,
                };
            }
        }
    }
}
