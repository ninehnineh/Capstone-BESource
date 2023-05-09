using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.CreateNewPackagePrice
{
    public class CreateNewPackagePriceCommandValidation : AbstractValidator<CreateNewPackagePriceCommand>
    {
        public CreateNewPackagePriceCommandValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Vui lòng nhập {Tên gói}.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Tên gói} không được nhập quá 50 kí tự");
            RuleFor(c => c.Price)
                .NotEmpty().WithMessage("Vui lòng nhập {Giá gói}.")
                .NotNull()
                .GreaterThan(0).WithMessage("{Giá gói} phải lớn hơn 0");
            RuleFor(c => c.Description)
               .NotEmpty().WithMessage("Vui lòng nhập {Mô tả}.")
               .NotNull()
               .MaximumLength(250).WithMessage("{Mô tả} không được nhập quá 250 kí tự");
            RuleFor(c => c.StartTime)
               .NotEmpty().WithMessage("Vui lòng nhập {Giờ bắt đầu}.")
               .NotNull();
            RuleFor(c => c.EndTime)
               .NotEmpty().WithMessage("Vui lòng nhập {Giờ kết thúc}.")
               .NotNull();
            RuleFor(c => c.TrafficId)
                .NotEmpty().WithMessage("Vui lòng nhập {TrafficId}.")
                .NotNull();
        }
    }
}
