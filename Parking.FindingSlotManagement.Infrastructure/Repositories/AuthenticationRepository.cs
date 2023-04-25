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

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ParkZDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationRepository(IConfiguration configuration,
            IOptions<JwtSettings> jwtSettings,
            IHttpContextAccessor httpContextAccessor,
            ParkZDbContext DbContext)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = DbContext;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ServiceResponse<AuthResponse>> Login(AuthRequest request)
        {

            if (string.IsNullOrEmpty(request.Email.Trim()))
            {
                return new ServiceResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Vui lòng nhập email",
                    StatusCode = 400,
                };
            }

            if (!request.Email.Contains("@"))
            {
                return new ServiceResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Email không hợp lệ, vui lòng nhập email",
                    StatusCode = 400,
                };
            }

            if (string.IsNullOrEmpty(request.Password.Trim()))
            {
                return new ServiceResponse<AuthResponse>
                {
                    Success = false,
                    Message = "vui lòng nhập password",
                    StatusCode = 400,
                };
            }

            if (request.Email.Count() > 50)
            {
                return new ServiceResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Email không được vượt quá 50 ký tự",
                    StatusCode = 400,
                };
            }

            var response = new ServiceResponse<AuthResponse>();

            var adminEmail = _configuration.GetSection("DefaultAccount").GetSection("Email").Value;
            var adminPassowrd = _configuration.GetSection("DefaultAccount").GetSection("Password").Value;

            if (request.Email.Equals(adminEmail) && request.Password.Equals(adminPassowrd))
            {
                return GenerateTokenForAdminNotManagedByPersistences(response, adminEmail);
            }

            var normalUser = await _dbContext.Users.Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email.Equals(request.Email) && x.Password.Equals(request.Password));


            if (normalUser!.IsActive == false)
            {
                return new ServiceResponse<AuthResponse>
                {
                    Message = "Tài khoản đã bị khóa",
                    StatusCode = 200,
                    Success = false,
                };
            }

            if (normalUser != null)
            {
                response.Success = true;
                response.Message = $"Chào mừng {normalUser.Name}";
                response.StatusCode = 200;
                response.Data = new AuthResponse
                {
                    Token = GenerateToken(normalUser).ToString(),
                };
                return response;
            }

            response.Success = false;
            response.Message = "Người dùng không tồn tại, đăng nhập thất bại";
            response.StatusCode = 404;

            return response;

        }

        private ServiceResponse<AuthResponse> GenerateTokenForAdminNotManagedByPersistences(
            ServiceResponse<AuthResponse> response, string adminEmail)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, adminEmail),
                    new Claim("Email", adminEmail),
                    new Claim("Role", "Admin")
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityTokenForAdmin = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            var identity = new ClaimsIdentity(claims, "custom");
            var principal = new ClaimsPrincipal(identity);

            _httpContextAccessor.HttpContext.User = principal;

            response.Success = true;
            response.Message = "Successfully";
            response.StatusCode = 200;
            response.Data = new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityTokenForAdmin),
            };
            return response;
        }

        private JwtSecurityToken GenerateToken(User user)
        {

            var roleClaims = new List<Claim>();

            roleClaims.Add(new Claim(ClaimTypes.Role, user.Role.Name));


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.UserId.ToString()),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}

