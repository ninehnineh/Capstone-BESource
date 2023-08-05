using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Domain.Entities;
using static Google.Apis.Requests.BatchRequest;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories.AuthenticationRepositories
{
    public class BusinessManagerAuthenticationRepository : IBusinessManagerAuthenticationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IParkingRepository _parkingRepository;
        private readonly ParkZDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public BusinessManagerAuthenticationRepository(IConfiguration configuration,
            IOptions<JwtSettings> jwtSettings,
            IHttpContextAccessor httpContextAccessor,
            IParkingRepository parkingRepository,
            ParkZDbContext DbContext)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _parkingRepository = parkingRepository;
            _dbContext = DbContext;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ServiceResponse<string>> Register(User user, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            if (string.IsNullOrEmpty(user.Email!.Trim()))
            {
                response.Success = false;
                response.Message = "Email không hợp lệ.";
                return response;
            }

            if (await UserExists(user.Email!))
            {
                response.Success = false;
                response.Message = "Email đã tồn tại.";
                return response;
            }

            CreatePasswordHash(password,
                out byte[] passwordHash,
                out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            response.Message = "Đăng ký thành công";
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _dbContext.Users.AnyAsync(x => x
            .Email!.ToLower().Equals(username.ToLower())))
                return true;
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<ServiceResponse<AuthResponse>> ManagerLogin(AuthRequest request)
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

            var manager = await _dbContext.Users.Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email!.Equals(request.Email));

            if (manager == null)
            {
                response.Success = false;
                response.Message = "Người dùng không tồn tại, đăng nhập thất bại";
                response.StatusCode = 404;

                return response;
            }
            else if (!PasswordManage.VerifyPasswordHash(request.Password, manager.PasswordHash!, manager.PasswordSalt!))
            {
                response.Success = false;
                response.Message = "Sai mật khẩu";
                response.StatusCode = 404;

                return response;
            }

            if (manager!.IsActive == false)
            {
                return new ServiceResponse<AuthResponse>
                {
                    Message = "Tài khoản đã bị khóa",
                    StatusCode = 200,
                    Success = false,
                };
            }

            /*if (manager.IsCensorship == false)
            {
                response.Success = false;
                response.Message = "Tài khoản doanh nghiệp đang chờ xét duyệt. Vui lòng đợi email xác nhận";
                response.StatusCode = 404;

                return response;
            }*/

            TokenManage token = new TokenManage(_jwtSettings, _configuration);

            if (manager.IsActive == true && manager.IsCensorship == true && manager.Role!.Name!.Equals("Manager") || manager.IsActive == true && manager.IsCensorship == true && manager.Role!.Name!.Equals("Keeper"))
            {
                if(manager.Role!.Name!.Equals("Keeper"))
                {
                    var parkingExist = await _parkingRepository.GetById(manager.ParkingId);
                    if (parkingExist == null)
                    {
                        return new ServiceResponse<AuthResponse>
                        {
                            Message = "Không tìm thấy bãi.",
                            Success = false,
                            StatusCode = 404
                        };
                    }
                    if(parkingExist.IsAvailable == false)
                    {
                        return new ServiceResponse<AuthResponse>
                        {
                            Message = "Bãi đang bị quản lý khóa.",
                            Success = false,
                            StatusCode = 400
                        };
                    }
                }

                response.Success = true;
                response.Message = $"Chào mừng {manager.Name}";
                response.StatusCode = 200;
                response.Data = new AuthResponse
                {
                    Token = token.GenerateToken(manager).ToString()!,
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

        public Task<ServiceResponse<string>> ManagerRegister(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}

