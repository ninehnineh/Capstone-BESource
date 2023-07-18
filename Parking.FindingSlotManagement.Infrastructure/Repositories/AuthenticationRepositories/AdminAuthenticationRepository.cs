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
        private readonly IStaffAuthenticationRepository _staffAuthenticationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ParkZDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public AdminAuthenticationRepository(IConfiguration configuration,
            IOptions<JwtSettings> jwtSettings,
            IStaffAuthenticationRepository staffAuthenticationRepository,
            IHttpContextAccessor httpContextAccessor,
            ParkZDbContext DbContext)
        {
            _configuration = configuration;
            _staffAuthenticationRepository = staffAuthenticationRepository;
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
            var staff = await _staffAuthenticationRepository.StaffLogin(request);
            /*response.Success = false;
            response.StatusCode = 404;
            response.Message = "Đăng nhập thất bại";*/

            return staff;
        }

        private ServiceResponse<AuthResponse> GenerateTokenForAdminNotManagedByPersistences(
            ServiceResponse<AuthResponse> response, string adminEmail)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, adminEmail),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var accessToken = tokenHandler.WriteToken(token);
            response.Success = true;
            response.Message = "Successfully";
            response.StatusCode = 200;
            response.Data = new AuthResponse
            {
                Token = accessToken,
            };
            return response;
        }

        public Task<ServiceResponse<string>> AdminRegister(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
