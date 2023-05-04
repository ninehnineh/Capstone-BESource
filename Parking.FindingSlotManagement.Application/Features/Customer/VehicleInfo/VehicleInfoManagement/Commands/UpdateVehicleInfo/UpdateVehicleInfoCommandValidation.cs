using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Commands.UpdateVehicleInfo
{
    public class UpdateVehicleInfoCommandValidation : AbstractValidator<UpdateVehicleInfoCommand>
    {
        public UpdateVehicleInfoCommandValidation()
        {
            RuleFor(p => p.LicensePlate)
                .MaximumLength(15).WithMessage("{Biển số xe} không được nhập quá 15 kí tự");
            RuleFor(p => p.VehicleName)
                .MaximumLength(50).WithMessage("{Tên xe} không được nhập quá 50 kí tự");
            RuleFor(p => p.Color)
                .MaximumLength(50).WithMessage("{Màu} không được nhập quá 50 kí tự");
        }
    }
}
