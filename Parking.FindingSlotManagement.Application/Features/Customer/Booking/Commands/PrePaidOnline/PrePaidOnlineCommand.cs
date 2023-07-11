using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.PrePaidOnline
{
    public class PrePaidOnlineCommand : IRequest<ServiceResponse<string>>
    {
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int ParkingId { get; set; }
    }
}
