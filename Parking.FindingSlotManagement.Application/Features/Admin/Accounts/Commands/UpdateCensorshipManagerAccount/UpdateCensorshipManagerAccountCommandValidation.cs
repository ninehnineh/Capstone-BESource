using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.Commands.UpdateCensorshipManagerAccount
{
    public class UpdateCensorshipManagerAccountCommandValidation : AbstractValidator<UpdateCensorshipManagerAccountCommand>
    {
        public UpdateCensorshipManagerAccountCommandValidation()
        {
            RuleFor(p => p.Name)
                .MaximumLength(30).WithMessage("{Name} must have less than 30 characters");
            RuleFor(p => p.Email)
                .EmailAddress().WithMessage("Email is not valid.")
                .MaximumLength(50).WithMessage("{Email} must have less than 50 characters");
            RuleFor(p => p.Phone)
                /*.Must(x => int.TryParse(x, out _)).WithMessage("Phone must be numbers")*/
                .Length(10).WithMessage("{Phone} must have 10 numbers");
            RuleFor(p => p.DateOfBirth)
                .LessThanOrEqualTo(DateTime.UtcNow.AddHours(7)).WithMessage("{DateOfBirth} must be less than current date");
            RuleFor(p => p.Gender)
                .MaximumLength(10).WithMessage("{Gender} must have less than 10 characters");
        }
    }
}
