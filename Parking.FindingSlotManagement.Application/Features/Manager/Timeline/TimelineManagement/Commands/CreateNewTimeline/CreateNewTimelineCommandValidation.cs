using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.CreateNewTimeline
{
    public class CreateNewTimelineCommandValidation : AbstractValidator<CreateNewTimelineCommand>
    {
        public CreateNewTimelineCommandValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Vui lòng nhập {Tên gói}.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Tên gói} không được nhập quá 50 kí tự");
            RuleFor(c => c.Price)
                .NotEmpty().WithMessage("Vui lòng nhập {Giá tiền của tiếng khởi điểm}.")
                .NotNull()
                .GreaterThan(0).WithMessage("{Giá tiền của tiếng khởi điểm} phải lớn hơn 0");
            RuleFor(c => c.Description)
               .NotEmpty().WithMessage("Vui lòng nhập {Mô tả}.")
               .NotNull()
               .MaximumLength(250).WithMessage("{Mô tả} không được nhập quá 250 kí tự");
            RuleFor(c => c.StartTime)
               .NotEmpty().WithMessage("Vui lòng nhập {Giờ bắt đầu}.")
               .NotNull();
            RuleFor(c => c.EndTime)
               .NotEmpty().WithMessage("Vui lòng nhập {Giờ kết thúc}.")
               .NotNull();
            RuleFor(c => c.StartingTime)
                .NotEmpty().WithMessage("Vui lòng nhập {Số tiếng khởi điểm}.")
                .NotNull()
                .GreaterThanOrEqualTo(0).WithMessage("{Số tiếng khởi điểm} phải lớn hơn 0")
                .LessThan(24).WithMessage("{Số tiếng khởi điểm} phải nhỏ hơn hoặc bằng 24");
            RuleFor(c => c.TrafficId)
                .NotEmpty().WithMessage("Vui lòng nhập {TrafficId}.")
                .NotNull();
            RuleFor(c => c.ParkingPriceId)
                .NotEmpty().WithMessage("Vui lòng nhập {ParkingPriceId}.")
                .NotNull();
        }
    }
}
