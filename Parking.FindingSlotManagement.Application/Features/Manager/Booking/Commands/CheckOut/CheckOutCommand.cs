using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckOut
{
    public class CheckOutCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }
        public int ParkingId { get; set; }
        public int VehicleInfoId { get; set; }

    }
}
