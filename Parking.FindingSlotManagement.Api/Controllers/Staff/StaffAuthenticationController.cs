using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;

namespace Parking.FindingSlotManagement.Api.Controllers.Staff
{
    [Route("api/staff-authentication")]
    [ApiController]
    public class StaffAuthenticationController : ControllerBase
    {
        private readonly IStaffAuthenticationRepository _staffAuthenticationRepository;

        public StaffAuthenticationController(IStaffAuthenticationRepository staffAuthenticationRepository)
        {
            _staffAuthenticationRepository = staffAuthenticationRepository;
        }

        /// <summary>
        /// API for Staff
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<AuthResponse>>> StaffLogin(AuthRequest request)
        {
            try
            {
                var staff = await _staffAuthenticationRepository.StaffLogin(request);
                return StatusCode((int)staff.StatusCode, staff);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
