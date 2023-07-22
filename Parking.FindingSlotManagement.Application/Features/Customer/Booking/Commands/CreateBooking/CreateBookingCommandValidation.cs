using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandValidation : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidation()
        {


            RuleFor(x => x.BookingDto.StartTime)
                .NotNull()
                .NotEmpty().WithMessage("Vui lòng chọn 'Giờ vào'");

            RuleFor(x => x.BookingDto.EndTime)
                .NotNull()
                .NotEmpty().WithMessage("Vui lòng chọn 'Thời hạn'");

            RuleFor(x => x.BookingDto.GuestPhone)
                .Length(10).WithMessage("'Số điện thoại' không được vượt quá 10 số");

            RuleFor(x => x.BookingDto.GuestName)
                .MaximumLength(50).WithMessage("'Tên' không được vượt quá 50 ký tự");
        }
    }
}
