using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.CreateNewParkingSpotImage
{
    public class CreateNewParkingSpotImageCommandValidation : AbstractValidator<CreateNewParkingSpotImageCommand>
    {
        public CreateNewParkingSpotImageCommandValidation()
        {
            RuleFor(p => p.ImgPath)
                .NotEmpty().WithMessage("Vui lòng nhập {ImgPath}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{ImgPath} không được nhập quá 250 kí tự");
            RuleFor(p => p.ParkingId)
                .NotEmpty().WithMessage("Vui lòng nhập {ParkingId}.")
                .NotNull();
        }
    }
}
