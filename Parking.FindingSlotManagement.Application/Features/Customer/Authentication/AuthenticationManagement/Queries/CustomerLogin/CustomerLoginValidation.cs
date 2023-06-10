using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Queries.CustomerLogin
{
    public class CustomerLoginValidation : AbstractValidator<CustomerLoginQuery>
    {
        public CustomerLoginValidation()
        {
            RuleFor(p => p.Phone)
                .NotEmpty().WithMessage("Vui lòng nhập {Phone}.")
                .NotNull()
                .Must(x => int.TryParse(x, out _)).WithMessage("{Phone} là chữ số.")
                .Length(10).WithMessage("{Phone} cần phải nhập 10 chữ số.");
        }
    }
}
