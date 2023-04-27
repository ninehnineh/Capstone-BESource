using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.UpdateFavoriteAddress
{
    public class UpdateFavoriteAddressCommandValidation : AbstractValidator<UpdateFavoriteAddressCommand>
    {
        public UpdateFavoriteAddressCommandValidation()
        {
            RuleFor(p => p.TagName)
                .MaximumLength(20).WithMessage("{Tên nhãn} không được nhập quá 30 kí tự");
            RuleFor(p => p.Address)
                .MaximumLength(250).WithMessage("{Địa chỉ} không được nhập quá 250 kí tự");
        }
    }
}
