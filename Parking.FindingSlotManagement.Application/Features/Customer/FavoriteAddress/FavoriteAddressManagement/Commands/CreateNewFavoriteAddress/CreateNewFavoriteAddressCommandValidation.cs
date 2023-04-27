using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.CreateNewFavoriteAddress
{
    public class CreateNewFavoriteAddressCommandValidation : AbstractValidator<CreateNewFavoriteAddressCommand>
    {
        public CreateNewFavoriteAddressCommandValidation()
        {
            RuleFor(p => p.TagName)
                .NotEmpty().WithMessage("Vui lòng nhập {Tên nhãn}.")
                .NotNull()
                .MaximumLength(20).WithMessage("{Tên nhãn} không được nhập quá 30 kí tự");
            RuleFor(p => p.Address)
                .NotEmpty().WithMessage("Vui lòng nhập {Địa chỉ}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Địa chỉ} không được nhập quá 250 kí tự");
        }
    }
}
