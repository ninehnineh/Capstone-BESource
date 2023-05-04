using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.UpdateParking
{
    public class UpdateParkingCommandValidation : AbstractValidator<UpdateParkingCommand>
    {
        public UpdateParkingCommandValidation()
        {
            RuleFor(p => p.Name)
                .MaximumLength(50).WithMessage("{Tên bãi xe} không được nhập quá 50 kí tự");
            RuleFor(p => p.Address)
                .MaximumLength(250).WithMessage("{Địa chỉ} không được nhập quá 250 kí tự");
            RuleFor(p => p.Description)
                .MaximumLength(250).WithMessage("{Mô tả} không được nhập quá 250 kí tự");
        }
    }
}
