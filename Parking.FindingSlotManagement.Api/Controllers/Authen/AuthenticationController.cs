using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Application;

namespace Parking.FindingSlotManagement.Api.Controllers.Authen
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationController(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<AuthResponse>>> Login(AuthRequest request)
        {
            var user = await _authenticationRepository.Login(request);
            return StatusCode((int)user.StatusCode, user);
        }
    }
}
