using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.UpdateLocationOfParking
{
    public class UpdateLocationCommandValidation : AbstractValidator<UpdateLocationCommand>
    {
        public UpdateLocationCommandValidation()
        {
            RuleFor(p => p.Latitude)
                .NotEmpty().WithMessage("Vui lòng nhập {Vĩ độ}.")
                .NotNull();
            RuleFor(p => p.Longitude)
                .NotEmpty().WithMessage("Vui lòng nhập {Kinh độ}.")
                .NotNull();
        }
    }
}
