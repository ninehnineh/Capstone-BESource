using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PostPaidOnline
{
    public class PostPaidOnlineCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; }
        public int ParkingId { get; set; }
        public HttpContext? context { get; set; }
    }
}
