using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.PasswordManagement.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<ServiceResponse<string>>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
