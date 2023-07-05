using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.ChangeStatusToAlreadyPaid
{
    public class ChangeStatusToAlreadyPaidCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }
        public int ParkingId { get; set; }
    }
}
