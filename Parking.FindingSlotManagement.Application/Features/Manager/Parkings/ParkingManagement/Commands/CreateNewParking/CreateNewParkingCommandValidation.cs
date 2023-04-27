using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CreateNewParking
{
    public class CreateNewParkingCommandValidation : AbstractValidator<CreateNewParkingCommand>
    {
        public CreateNewParkingCommandValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Vui lòng nhập {Tên bãi xe}.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Tên bãi xe} không được nhập quá 50 kí tự");
            RuleFor(p => p.Address)
                .NotEmpty().WithMessage("Vui lòng nhập {Địa chỉ}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Địa chỉ} không được nhập quá 250 kí tự");
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Vui lòng nhập {Mô tả}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{Mô tả} không được nhập quá 250 kí tự");
            RuleFor(p => p.MotoSpot)
                .GreaterThanOrEqualTo(0).WithMessage("{Số slot xe máy} phải lớn hơn bằng 0");
            RuleFor(p => p.CarSpot)
                .GreaterThanOrEqualTo(0).WithMessage("{Số slot xe ô tô} phải lớn hơn bằng 0");
            RuleFor(p => p.IsPrepayment)
                .NotEmpty().WithMessage("Vui lòng chọn {Có thanh toán trả trước} hay không?")
                .NotNull();
            RuleFor(p => p.IsOvernight)
                .NotEmpty().WithMessage("Vui lòng chọn {Có áp dụng qua giữ xe qua đêm} hay không?")
                .NotNull();
            RuleFor(p => p.ManagerId)
                .NotEmpty().WithMessage("Vui lòng nhập {ManagerId}.")
                .NotNull();
        }
    }
}
