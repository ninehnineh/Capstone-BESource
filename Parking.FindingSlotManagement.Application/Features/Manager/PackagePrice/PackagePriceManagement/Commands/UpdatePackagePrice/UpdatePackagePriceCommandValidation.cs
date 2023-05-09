using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.UpdatePackagePrice
{
    public class UpdatePackagePriceCommandValidation : AbstractValidator<UpdatePackagePriceCommand>
    {
        public UpdatePackagePriceCommandValidation()
        {
            RuleFor(c => c.Name)
                .MaximumLength(50).WithMessage("{Tên gói} không được nhập quá 50 kí tự");
            RuleFor(c => c.Price)
                .GreaterThan(0).WithMessage("{Giá gói} phải lớn hơn 0");
            RuleFor(c => c.Description)
               .MaximumLength(250).WithMessage("{Mô tả} không được nhập quá 250 kí tự");
        }
    }
}
