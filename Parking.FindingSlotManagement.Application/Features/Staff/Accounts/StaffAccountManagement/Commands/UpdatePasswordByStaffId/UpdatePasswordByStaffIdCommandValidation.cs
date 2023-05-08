using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.Accounts.StaffAccountManagement.Commands.UpdatePasswordByStaffId
{
    public class UpdatePasswordByStaffIdCommandValidation : AbstractValidator<UpdatePasswordByStaffIdCommand>
    {
        public UpdatePasswordByStaffIdCommandValidation()
        {
            RuleFor(p => p.OldPassword)
                .NotEmpty().WithMessage("Vui lòng nhập {Mật khẩu cũ}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Mật khẩu cũ} không được nhập quá 250 kí tự");
            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập {Mật khẩu mới}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Mật khẩu mới} không được nhập quá 250 kí tự");
        }
    }
}
