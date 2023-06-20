using MediatR;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBookingWhenAlreadyPaid
{
    public class CreateBookingWhenAlreadyPaidCommand : IRequest<ServiceResponse<int>>
    {
        public BookingDto BookingDto { get; set; }
        public string DeviceToKenMobile { get; set; }
    }
}
