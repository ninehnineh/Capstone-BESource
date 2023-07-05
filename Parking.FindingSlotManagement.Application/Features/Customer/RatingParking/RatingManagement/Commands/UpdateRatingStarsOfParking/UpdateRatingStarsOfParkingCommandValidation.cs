using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.RatingParking.RatingManagement.Commands.UpdateRatingStarsOfParking
{
    public class UpdateRatingStarsOfParkingCommandValidation : AbstractValidator<UpdateRatingStarsOfParkingCommand>
    {
        public UpdateRatingStarsOfParkingCommandValidation()
        {
            RuleFor(p => p.ParkingId)
                .NotEmpty().WithMessage("Vui lòng nhập {ParkingId}.")
                .NotNull();
            RuleFor(p => p.Stars)
                .NotEmpty().WithMessage("Vui lòng nhập {Số sao}.")
                .NotNull()
                .GreaterThan(0).WithMessage("{Số sao} phải lớn hơn 0")
                .LessThanOrEqualTo(5).WithMessage("{Số sao} phải nhỏ hơn hoặc bằng 5");
        }
    }
}
