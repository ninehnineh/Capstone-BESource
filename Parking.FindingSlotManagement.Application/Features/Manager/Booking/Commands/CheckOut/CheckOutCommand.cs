using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckOut
{
    public class CheckOutCommand : IRequest<ServiceResponse<string>>
    {
        public int BookingId { get; set; }
        public int ParkingId { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? PaymentMethod { get; set; }
        // totalprice (unpaid) = 0 va paymentmethod = null

        // paymentmethod = online
        // paymentmethod = tien mat
    }
}
