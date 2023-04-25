using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.Commands.CreateNewCensorshipManagerAccount
{
    public class CreateNewCensorshipManagerAccountCommandValidation : AbstractValidator<CreateNewCensorshipManagerAccountCommand>
    {
        public CreateNewCensorshipManagerAccountCommandValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{Name} is required.")
                .NotNull()
                .MaximumLength(30).WithMessage("{Name} must have less than 30 characters");
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{Email} is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Email} must have less than 50 characters");
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("{Password} is required.")
                .NotNull();
            RuleFor(p => p.Phone)
                .NotEmpty().WithMessage("{Phone} is required.")
                .NotNull()
                .Must(x => int.TryParse(x, out _)).WithMessage("Phone must be numbers")
                .Length(10).WithMessage("{Phone} must have 10 numbers");
            RuleFor(p => p.Avatar)
                .NotEmpty().WithMessage("{Avatar} is required.")
                .NotNull();
            RuleFor(p => p.DateOfBirth)
                .NotEmpty().WithMessage("{DateOfBirth} is required.")
                .NotNull()
                .LessThanOrEqualTo(DateTime.UtcNow.AddHours(7)).WithMessage("{DateOfBirth} must be less than current date");
            RuleFor(p => p.Gender)
                .NotEmpty().WithMessage("{Gender} is required.")
                .NotNull()
                .MaximumLength(10).WithMessage("{Gender} must have less than 10 characters");
        }
    }
}
