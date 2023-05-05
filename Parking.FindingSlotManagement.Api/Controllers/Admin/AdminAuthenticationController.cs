using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;

namespace Parking.FindingSlotManagement.Api.Controllers.Admin
{
    [Route("api/admin-authentication")]
    [ApiController]
    public class AdminAuthenticationController : ControllerBase
    {
        private readonly IAdminAuthenticationRepository _adminAuthenticationRepository;

        public AdminAuthenticationController(IAdminAuthenticationRepository adminAuthenticationRepository)
        {
            _adminAuthenticationRepository = adminAuthenticationRepository;
        }

        /// <summary>
        /// API for Admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<AuthResponse>>> AdminLogin(AuthRequest request)
        {
            try
            {
                var admin = await _adminAuthenticationRepository.AdminLogin(request);
                return StatusCode((int)admin.StatusCode, admin);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
