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
        }
    }
}
