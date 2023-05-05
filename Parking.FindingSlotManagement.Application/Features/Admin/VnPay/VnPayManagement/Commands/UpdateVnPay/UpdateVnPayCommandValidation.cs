using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.UpdateVnPay
{
    public class UpdateVnPayCommandValidation : AbstractValidator<UpdateVnPayCommand>
    {
        public UpdateVnPayCommandValidation()
        {
            RuleFor(x => x.TmnCode)
                .MaximumLength(20).WithMessage("{TmnCode} không được nhập quá 20 kí tự");
            RuleFor(x => x.HashSecret)
                .MaximumLength(250).WithMessage("{HashSecret} không được nhập quá 250 kí tự");
        }
    }
}
