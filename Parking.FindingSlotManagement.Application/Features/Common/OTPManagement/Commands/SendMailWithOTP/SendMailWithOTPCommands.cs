using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.OTPManagement.Commands.SendMailWithOTP
{
    public class SendMailWithOTPCommands : IRequest<ServiceResponse<string>>
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}
