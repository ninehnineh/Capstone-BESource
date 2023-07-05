using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckIn
{
    public class CheckInCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }
    }
}
