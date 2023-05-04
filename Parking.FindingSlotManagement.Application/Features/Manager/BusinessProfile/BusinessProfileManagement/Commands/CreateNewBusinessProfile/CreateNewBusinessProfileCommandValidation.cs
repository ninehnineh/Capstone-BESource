using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Commands.CreateNewBusinessProfile
{
    public class CreateNewBusinessProfileCommandValidation : AbstractValidator<CreateNewBusinessProfileCommand>
    {
        public CreateNewBusinessProfileCommandValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Vui lòng nhập {Tên doanh nghiệp}.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Tên doanh nghiệp} không được nhập quá 50 kí tự");
            RuleFor(p => p.Address)
                .NotEmpty().WithMessage("Vui lòng nhập {Địa chỉ}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Địa chỉ} không được nhập quá 250 kí tự");
            RuleFor(p => p.FrontIdentification)
                .NotEmpty().WithMessage("Vui lòng nhập {Mặt trước của căn cước công dân}.")
                .NotNull();
            RuleFor(p => p.BackIdentification)
                .NotEmpty().WithMessage("Vui lòng nhập {Mặt sau của căn cước công dân}.")
                .NotNull();
            RuleFor(p => p.BusinessLicense)
                .NotEmpty().WithMessage("Vui lòng nhập {Giấy phép kinh doanh}.")
                .NotNull();
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("Vui lòng nhập {UserId}.")
                .NotNull();
        }
    }
}
