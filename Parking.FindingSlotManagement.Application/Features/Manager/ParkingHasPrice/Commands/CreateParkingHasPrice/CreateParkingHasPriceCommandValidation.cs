using FluentValidation;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.CreateParkingHasPrice
{
    public class CreateParkingHasPriceCommandValidation : AbstractValidator<CreateParkingHasPriceCommand>
    {

        public CreateParkingHasPriceCommandValidation()
        {

            RuleFor(x => x.ParkingId)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}")
                .NotNull()
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải lớn hơn 0");

            RuleFor(x => x.ParkingPriceId)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}")
                .NotNull()
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải lớn hơn 0");

        }
    }
}
