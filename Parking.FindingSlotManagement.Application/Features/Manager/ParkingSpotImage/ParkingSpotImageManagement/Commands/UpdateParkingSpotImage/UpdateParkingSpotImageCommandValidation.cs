using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.UpdateParkingSpotImage
{
    public class UpdateParkingSpotImageCommandValidation : AbstractValidator<UpdateParkingSpotImageCommand>
    {
        public UpdateParkingSpotImageCommandValidation()
        {
            RuleFor(p => p.ImgPath)
                .MaximumLength(250).WithMessage("{ImgPath} không được nhập quá 250 kí tự");
        }
    }
}
