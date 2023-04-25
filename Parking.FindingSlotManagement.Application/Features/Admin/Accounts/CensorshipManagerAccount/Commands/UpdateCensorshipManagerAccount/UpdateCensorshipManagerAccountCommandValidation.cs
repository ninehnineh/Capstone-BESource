using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.UpdateCensorshipManagerAccount
{
    public class UpdateCensorshipManagerAccountCommandValidation : AbstractValidator<UpdateCensorshipManagerAccountCommand>
    {
        public UpdateCensorshipManagerAccountCommandValidation()
        {
            RuleFor(p => p.Name)
                .MaximumLength(30).WithMessage("{Name} không được nhập quá 30 kí tự");
            RuleFor(p => p.Email)
                .EmailAddress().WithMessage("Email không hợp lệ.")
                .MaximumLength(50).WithMessage("{Email} không được nhập quá 50 kí tự");
            RuleFor(p => p.Phone)
                /*.Must(x => int.TryParse(x, out _)).WithMessage("Phone must be numbers")*/
                .Length(10).WithMessage("{Phone} cần phải nhập 10 chữ số.");
            RuleFor(p => p.DateOfBirth)
                .LessThanOrEqualTo(DateTime.UtcNow.AddHours(7)).WithMessage("{DateOfBirth} cần phải nhỏ hơn ngày hiện tại.");
            RuleFor(p => p.Gender)
                .MaximumLength(10).WithMessage("{Gender} không được nhập quá 10 kí tự");
        }
    }
}
