using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.UpdateFloor
{
    public class UpdateFloorCommandValidation : AbstractValidator<UpdateFloorCommand>
    {
        public UpdateFloorCommandValidation()
        {
            RuleFor(p => p.FloorName)
                .MaximumLength(15).WithMessage("{Tên tầng} không được nhập quá 15 kí tự");
        }
    }
}
