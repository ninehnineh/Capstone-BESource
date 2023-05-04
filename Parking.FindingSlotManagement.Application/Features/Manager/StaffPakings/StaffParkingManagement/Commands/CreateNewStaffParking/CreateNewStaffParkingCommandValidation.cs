using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.StaffPakings.StaffParkingManagement.Commands.CreateNewStaffParking
{
    public class CreateNewStaffParkingCommandValidation : AbstractValidator<CreateNewStaffParkingCommand>
    {
        public CreateNewStaffParkingCommandValidation()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("Vui lòng nhập {UserId}.")
                .NotNull();
            RuleFor(p => p.ParkingId)
                .NotEmpty().WithMessage("Vui lòng nhập {ParkingId}.")
                .NotNull();
        }
    }
}
