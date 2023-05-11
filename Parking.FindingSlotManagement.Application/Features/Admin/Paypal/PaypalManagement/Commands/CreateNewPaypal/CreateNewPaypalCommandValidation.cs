using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.CreateNewPaypal
{
    public class CreateNewPaypalCommandValidation : AbstractValidator<CreateNewPaypalCommand>
    {
        public CreateNewPaypalCommandValidation()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Vui lòng nhập {ClientId}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{ClientId} không được nhập quá 250 kí tự");
            RuleFor(x => x.SecretKey)
                .NotEmpty().WithMessage("Vui lòng nhập {SecretKey}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{SecretKey} không được nhập quá 250 kí tự");
            RuleFor(x => x.ManagerId)
                .NotEmpty().WithMessage("Vui lòng nhập {ManagerId}.")
                .NotNull();
        }
    }
}
