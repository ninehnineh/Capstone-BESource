using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.CreateNewFee
{
    public class CreateNewFeeCommandValidation : AbstractValidator<CreateNewFeeCommand>
    {
        public CreateNewFeeCommandValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Vui lòng nhập {Name}.")
                .NotNull()
                .MaximumLength(100).WithMessage("{Name} không được nhập quá 100 kí tự");
            RuleFor(p => p.BusinessType)
                .NotEmpty().WithMessage("Vui lòng nhập {BusinessType}.")
                .NotNull()
                .MaximumLength(100).WithMessage("{BusinessType} không được nhập quá 100 kí tự");
            RuleFor(price => price.Price)
                .NotEmpty().WithMessage("Vui lòng nhập {Price}.")
                .NotNull()
                .GreaterThan(0).WithMessage("{Price} phải lớn hơn 0.");
        }
    }
}
