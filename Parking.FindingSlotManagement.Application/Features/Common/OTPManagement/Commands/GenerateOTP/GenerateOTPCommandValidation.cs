using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.OTPManagement.Commands.GenerateOTP
{
    public class GenerateOTPCommandValidation : AbstractValidator<GenerateOTPCommand>
    {
        public GenerateOTPCommandValidation()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Vui lòng nhập {Email}.")
                .EmailAddress().WithMessage("Email không đúng định dạng.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Email} không được nhập quá 50 kí tự");
        }
    }
}
