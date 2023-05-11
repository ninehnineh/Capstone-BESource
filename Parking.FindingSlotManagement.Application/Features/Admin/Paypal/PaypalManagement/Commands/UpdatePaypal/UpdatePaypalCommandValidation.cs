using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.UpdatePaypal
{
    public class UpdatePaypalCommandValidation : AbstractValidator<UpdatePaypalCommand>
    {
        public UpdatePaypalCommandValidation()
        {
            RuleFor(x => x.ClientId)
                .MaximumLength(250).WithMessage("{ClientId} không được nhập quá 250 kí tự");
            RuleFor(x => x.SecretKey)
                .MaximumLength(250).WithMessage("{SecretKey} không được nhập quá 250 kí tự");
        }
    }
}
