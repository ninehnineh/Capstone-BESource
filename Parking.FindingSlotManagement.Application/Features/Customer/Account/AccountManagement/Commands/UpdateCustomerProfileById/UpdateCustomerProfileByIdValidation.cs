using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Commands.UpdateCustomerProfileById
{
    public class UpdateCustomerProfileByIdValidation : AbstractValidator<UpdateCustomerProfileByIdCommand>
    {
        public UpdateCustomerProfileByIdValidation()
        {
            RuleFor(p => p.Name)
                .MaximumLength(30).WithMessage("{Name} không được nhập quá 30 kí tự");
            RuleFor(p => p.DateOfBirth)
                .LessThanOrEqualTo(DateTime.UtcNow.AddHours(7)).WithMessage("{DateOfBirth} cần phải nhỏ hơn ngày hiện tại.");
            RuleFor(p => p.Gender)
                .MaximumLength(10).WithMessage("{Gender} không được nhập quá 10 kí tự");
            RuleFor(p => p.Address)
                .MaximumLength(250).WithMessage("{Address} không được nhập quá 250 kí tự");
            RuleFor(p => p.Avatar)
                .MaximumLength(250).WithMessage("{Avatar} không được nhập quá 250 kí tự");
        }
    }
}
