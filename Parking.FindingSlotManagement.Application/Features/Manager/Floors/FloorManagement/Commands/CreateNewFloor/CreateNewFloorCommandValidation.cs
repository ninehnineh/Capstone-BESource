using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.CreateNewFloor
{
    public class CreateNewFloorCommandValidation : AbstractValidator<CreateNewFloorCommand>
    {
        public CreateNewFloorCommandValidation()
        {
            RuleFor(p => p.FloorName)
                .NotEmpty().WithMessage("Vui lòng nhập {Tên tầng}.")
                .NotNull()
                .MaximumLength(15).WithMessage("{Tên tầng} không được nhập quá 15 kí tự");
            RuleFor(p => p.ParkingId)
                .NotEmpty().WithMessage("Vui lòng nhập {Parking Id}.")
                .NotNull();
        }
    }
}
