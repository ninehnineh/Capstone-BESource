using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Commands.CustomerRegister
{
    public class CustomerRegisterCommandValidation : AbstractValidator<CustomerRegisterCommand>
    {
        public CustomerRegisterCommandValidation()
        {
            RuleFor(p => p.Phone)
                .NotEmpty().WithMessage("Vui lòng nhập {Phone}.")
                .NotNull()
                .Must(x => int.TryParse(x, out _)).WithMessage("{Phone} là chữ số.")
                .Length(10).WithMessage("{Phone} cần phải nhập 10 chữ số.");
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Vui lòng nhập {Name}.")
                .NotNull()
                .MaximumLength(30).WithMessage("{Name} không được nhập quá 30 kí tự");
            RuleFor(p => p.DateOfBirth)
                .NotEmpty().WithMessage("Vui lòng nhập {DateOfBirth}.")
                .NotNull()
                .LessThanOrEqualTo(DateTime.UtcNow.AddHours(7)).WithMessage("{DateOfBirth} cần phải nhỏ hơn ngày hiện tại");
            RuleFor(p => p.Gender)
                .NotEmpty().WithMessage("Vui lòng nhập {Gender}.")
                .NotNull()
                .MaximumLength(10).WithMessage("{Gender} không được nhập quá 10 kí tự");
            RuleFor(p => p.Address)
                .NotEmpty().WithMessage("Vui lòng nhập {Address}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Address} không được nhập quá 250 kí tự");
            RuleFor(p => p.IdCardNo)
                .NotEmpty().WithMessage("Vui lòng nhập {IdCardNo}.")
                .NotNull()
                .MaximumLength(20).WithMessage("{IdCardNo} không được nhập quá 20 kí tự");
            RuleFor(p => p.IdCardIssuedBy)
                .NotEmpty().WithMessage("Vui lòng nhập {IdCardIssuedBy}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{IdCardIssuedBy} không được nhập quá 250 kí tự");
            RuleFor(p => p.IdCardDate)
                .NotEmpty().WithMessage("Vui lòng nhập {IdCardDate}.")
                .NotNull()
                .LessThanOrEqualTo(DateTime.UtcNow.AddHours(7)).WithMessage("{IdCardDate} cần phải nhỏ hơn ngày hiện tại");
        }
    }
}
