using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.PasswordManagement.Commands.ForgotPassword
{
    public class ForgotPasswordCommandValidation : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidation()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Vui lòng nhập {Email}.")
                .EmailAddress().WithMessage("Email không đúng định dạng.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Email} không được nhập quá 50 kí tự");
            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập {Mật khẩu mới}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Mật khẩu mới} không được nhập quá 250 kí tự");
        }
    }
}
