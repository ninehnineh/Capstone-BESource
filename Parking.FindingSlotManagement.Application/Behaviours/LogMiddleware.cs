using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Behaviours
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var method = request.Method;
            var path = request.Path;
            var queryString = request.QueryString;

            _logger.LogInformation($"[{DateTime.Now}] {method} request to {path}{queryString}");

            await _next.Invoke(context);

            var response = context.Response;
            var statusCode = response.StatusCode;

            _logger.LogInformation($"[{DateTime.Now}] Response with {statusCode} status code");
        }
    }
}
