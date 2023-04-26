using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.CreateNewTraffic
{
    public class CreateNewTrafficCommandValidation : AbstractValidator<CreateNewTrafficCommand>
    {
        public CreateNewTrafficCommandValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Vui lòng nhập {Tên phương tiện}.")
                .NotNull()
                .MaximumLength(30).WithMessage("{Tên phương tiện} không được nhập quá 30 kí tự");
        }
    }
}
