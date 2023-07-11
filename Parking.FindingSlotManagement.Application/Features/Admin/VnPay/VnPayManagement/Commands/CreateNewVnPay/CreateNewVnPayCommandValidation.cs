using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.CreateNewVnPay
{
    public class CreateNewVnPayCommandValidation : AbstractValidator<CreateNewVnPayCommand>
    {
        public CreateNewVnPayCommandValidation()
        {
            RuleFor(x => x.TmnCode)
                .NotEmpty().WithMessage("Vui lòng nhập {TmnCode}.")
                .NotNull()
                .MaximumLength(20).WithMessage("{TmnCode} không được nhập quá 20 kí tự");
            RuleFor(x => x.HashSecret)
                .NotEmpty().WithMessage("Vui lòng nhập {HashSecret}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{HashSecret} không được nhập quá 250 kí tự");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Vui lòng nhập {UserId}.")
                .NotNull();
        }
    }
}
