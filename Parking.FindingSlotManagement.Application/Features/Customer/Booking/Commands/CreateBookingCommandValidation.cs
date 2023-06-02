using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands
{
    public class CreateBookingCommandValidation : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidation()
        {


            RuleFor(x => x.StartTime)
                .NotNull()
                .NotEmpty().WithMessage("Vui lòng chọn 'Giờ vào'");

            RuleFor(x => x.EndTime)
                .NotNull()
                .NotEmpty().WithMessage("Vui lòng chọn 'Thời hạn'");

            RuleFor(x => x.GuestPhone)
                .Length(10).WithMessage("'Số điện thoại' không được vượt quá 10 số");

            RuleFor(x => x.GuestName)
                .Length(50).WithMessage("'Tên' không được vượt quá 50 ký tự");
        }
    }
}
