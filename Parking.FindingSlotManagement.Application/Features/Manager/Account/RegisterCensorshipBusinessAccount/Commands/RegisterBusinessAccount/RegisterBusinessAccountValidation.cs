using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Account.RegisterCensorshipBusinessAccount.Commands.RegisterBusinessAccount
{
    public class RegisterBusinessAccountValidation : AbstractValidator<RegisterBusinessAccountCommand>
    {
        public RegisterBusinessAccountValidation()
        {
            RuleFor(p => p.UserEntity.Name)
                .NotEmpty().WithMessage("Vui lòng nhập {Name}.")
                .NotNull()
                .MaximumLength(30).WithMessage("{Name} không được nhập quá 30 kí tự");
            RuleFor(p => p.UserEntity.Email)
                .NotEmpty().WithMessage("Vui lòng nhập {Email}.")
                .EmailAddress().WithMessage("Email không đúng định dạng.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Email} không được nhập quá 50 kí tự");
            RuleFor(p => p.UserEntity.Password)
                .NotEmpty().WithMessage("Vui lòng nhập {Password}.")
                .NotNull();
            RuleFor(p => p.UserEntity.Phone)
                .NotEmpty().WithMessage("Vui lòng nhập {Phone}.")
                .NotNull()
                .Must(x => int.TryParse(x, out _)).WithMessage("{Phone} là chữ số.")
                .Length(10).WithMessage("{Phone} cần phải nhập 10 chữ số.");
            RuleFor(p => p.UserEntity.Avatar)
                .NotEmpty().WithMessage("Vui lòng nhập {Avatar}")
                .NotNull();
            RuleFor(p => p.BusinessProfileEntity.Name)
                .NotEmpty().WithMessage("Vui lòng nhập {Tên doanh nghiệp}.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Tên doanh nghiệp} không được nhập quá 50 kí tự");
            RuleFor(p => p.BusinessProfileEntity.Address)
                .NotEmpty().WithMessage("Vui lòng nhập {Địa chỉ}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Địa chỉ} không được nhập quá 250 kí tự");
            RuleFor(p => p.BusinessProfileEntity.FrontIdentification)
                .NotEmpty().WithMessage("Vui lòng nhập {Mặt trước của căn cước công dân}.")
                .NotNull();
            RuleFor(p => p.BusinessProfileEntity.BackIdentification)
                .NotEmpty().WithMessage("Vui lòng nhập {Mặt sau của căn cước công dân}.")
                .NotNull();
        }
    }
}
