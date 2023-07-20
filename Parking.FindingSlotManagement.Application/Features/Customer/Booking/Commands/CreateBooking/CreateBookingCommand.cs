using MediatR;
using Parking.FindingSlotManagement.Application.Models.VehicleInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<ServiceResponse<int>>
    {
        public BookingDto BookingDto { get; set; }
        public string DeviceToKenMobile { get; set; }

    }
}
