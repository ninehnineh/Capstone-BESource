using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.OTPManagement.Commands.GenerateOTP
{
    public class GenerateOTPCommand : IRequest<ServiceResponse<string>>
    {
        public string Email { get; set; }
    }
}
