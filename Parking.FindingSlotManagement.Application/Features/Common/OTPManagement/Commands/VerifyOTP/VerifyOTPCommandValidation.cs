using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.OTPManagement.Commands.VerifyOTP
{
    public class VerifyOTPCommandValidation : AbstractValidator<VerifyOTPCommand>
    {
        public VerifyOTPCommandValidation()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Vui lòng nhập {Email}.")
                .EmailAddress().WithMessage("Email không đúng định dạng.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Email} không được nhập quá 50 kí tự");
            RuleFor(p => p.OtpCode)
                .NotEmpty().WithMessage("Vui lòng nhập {OTP}.")
                .NotNull()
                .Must(x => int.TryParse(x, out _)).WithMessage("{OTP} là chữ số.")
                .Length(6).WithMessage("Độ dài của {OTP} bắt buộc là 6 số.");
        }
    }
}
