using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories.AuthenticationRepositories
{
    public class AdminAuthenticationRepository : IAdminAuthenticationRepository
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ParkZDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public AdminAuthenticationRepository(IConfiguration configuration,
            IOptions<JwtSettings> jwtSettings,
            IHttpContextAccessor httpContextAccessor,
            ParkZDbContext DbContext)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = DbContext;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ServiceResponse<AuthResponse>> AdminLogin(AuthRequest request)
        {
            var response = new ServiceResponse<AuthResponse>();

            var adminEmail = _configuration.GetSection("DefaultAccount").GetSection("Email").Value;
            var adminPassowrd = _configuration.GetSection("DefaultAccount").GetSection("Password").Value;

            if (request.Email.Equals(adminEmail) && request.Password.Equals(adminPassowrd))
            {
                return GenerateTokenForAdminNotManagedByPersistences(response, adminEmail);
            }

            response.Success = false;
            response.StatusCode = 404;
            response.Message = "Đăng nhập thất bại";

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

            _httpContextAccessor.HttpContext!.User = principal;

            response.Success = true;
            response.Message = "Successfully";
            response.StatusCode = 200;
            response.Data = new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityTokenForAdmin),
            };
            return response;
        }

        public Task<ServiceResponse<string>> AdminRegister(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
