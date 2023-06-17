using MediatR;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetAvailableSlots;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetAvailableSlot
{
    public class GetAvailableSlotsQuery : IRequest<ServiceResponse<IEnumerable<GetAvailableSlotsResponse>>>
    {
        public int ParkingId { get; set; }
        public DateTime StartTimeBooking { get; set; }
        public int DesireHour { get; set; }
    }
}
