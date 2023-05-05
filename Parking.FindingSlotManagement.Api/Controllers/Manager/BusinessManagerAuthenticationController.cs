using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;

namespace Parking.FindingSlotManagement.Api.Controllers.Manager
{
    [Route("api/business-manager-authentication")]
    [ApiController]
    public class BusinessManagerAuthenticationController : ControllerBase
    {
        private readonly IBusinessManagerAuthenticationRepository _businessManagerAuthenticationRepository;

        public BusinessManagerAuthenticationController(
            IBusinessManagerAuthenticationRepository businessManagerAuthenticationRepository)
        {
            _businessManagerAuthenticationRepository = businessManagerAuthenticationRepository;
        }

        /// <summary>
        /// API for Manager
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<AuthResponse>>> ManagerLogin(AuthRequest request)
        {
            try
            {
                var manager = await _businessManagerAuthenticationRepository.ManagerLogin(request);
                return StatusCode((int)manager.StatusCode, manager);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
