using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.UpdateFee
{
    public class UpdateFeeCommandValidation : AbstractValidator<UpdateFeeCommand>
    {
        public UpdateFeeCommandValidation()
        {
            RuleFor(p => p.Name)
                .MaximumLength(100).WithMessage("{Name} không được nhập quá 100 kí tự");
            RuleFor(p => p.BusinessType)
                .MaximumLength(100).WithMessage("{BusinessType} không được nhập quá 100 kí tự");
            RuleFor(price => price.Price)
                .GreaterThan(0).WithMessage("{Price} phải lớn hơn 0.");
        }
    }
}
