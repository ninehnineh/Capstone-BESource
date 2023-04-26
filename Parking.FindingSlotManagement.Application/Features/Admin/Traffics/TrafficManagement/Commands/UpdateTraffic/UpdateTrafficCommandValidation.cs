using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.UpdateTraffic
{
    public class UpdateTrafficCommandValidation : AbstractValidator<UpdateTrafficCommand>
    {
        public UpdateTrafficCommandValidation()
        {
            RuleFor(p => p.Name)
                .MaximumLength(30).WithMessage("{Tên phương tiện} không được nhập quá 30 kí tự");
        }
    }
}
