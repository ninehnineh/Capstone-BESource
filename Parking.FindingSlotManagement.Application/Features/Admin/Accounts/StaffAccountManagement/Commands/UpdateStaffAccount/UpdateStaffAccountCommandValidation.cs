using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.UpdateStaffAccount
{
    public class UpdateStaffAccountCommandValidation : AbstractValidator<UpdateStaffAccountCommand>
    {
        public UpdateStaffAccountCommandValidation()
        {
            RuleFor(p => p.Name)
                .MaximumLength(30).WithMessage("{Name} không được nhập quá 30 kí tự");
            RuleFor(p => p.DateOfBirth)
                .LessThanOrEqualTo(DateTime.UtcNow.AddHours(7)).WithMessage("{DateOfBirth} cần phải nhỏ hơn ngày hiện tại.");
            RuleFor(p => p.Gender)
                .MaximumLength(10).WithMessage("{Gender} không được nhập quá 10 kí tự");
        }
    }
}
