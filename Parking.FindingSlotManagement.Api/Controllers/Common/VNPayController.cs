using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Models;

namespace Parking.FindingSlotManagement.Api.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public VNPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        [HttpGet]
        public PaymentResponseModel GetTransactions()
        {
            var res = _vnPayService.PaymentExecute(Request.Query);
            return res;
        }
    }
}
