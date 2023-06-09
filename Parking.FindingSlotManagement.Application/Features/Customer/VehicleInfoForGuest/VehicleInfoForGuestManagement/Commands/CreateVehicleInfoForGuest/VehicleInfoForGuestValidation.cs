using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Commands.CreateVehicleInfoForGuest
{
    public class VehicleInfoForGuestValidation : AbstractValidator<VehicleInfoForGuestCommand>
    {
        public VehicleInfoForGuestValidation()
        {
            RuleFor(p => p.LicensePlate)
                .NotEmpty().WithMessage("Vui lòng nhập {Biển số xe}.")
                .NotNull()
                .MaximumLength(15).WithMessage("{Biển số xe} không được nhập quá 15 kí tự");
            RuleFor(p => p.VehicleName)
                .NotEmpty().WithMessage("Vui lòng nhập {Tên xe}.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Tên xe} không được nhập quá 50 kí tự");
            RuleFor(p => p.Color)
                .NotEmpty().WithMessage("Vui lòng nhập {Màu}.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Màu} không được nhập quá 50 kí tự");
        }
    }
}
