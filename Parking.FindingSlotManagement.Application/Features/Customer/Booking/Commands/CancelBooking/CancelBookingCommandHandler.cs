/*using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Enum;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CancelBooking
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkingSlotRepository _parkingSlotRepository;

        public CancelBookingCommandHandler(IBookingRepository bookingRepository,
            IParkingSlotRepository parkingSlotRepository)
        {
            _bookingRepository = bookingRepository;
            _parkingSlotRepository = parkingSlotRepository;
        }
        public async Task<ServiceResponse<string>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingRepository
                    .GetItemWithCondition(x => x.BookingId == request.BookingId, null, false);

                var parkingSlot = await _parkingSlotRepository
                    .GetItemWithCondition(x => x.ParkingSlotId == booking.ParkingSlotId, null, false);

                if (booking.Status == BookingStatus.Initial.ToString())
                {
                    booking.Status = BookingStatus.Cancel.ToString();
                    await _bookingRepository.Save();
                    parkingSlot.IsAvailable = true;
                    await _parkingSlotRepository.Save();

                    return new ServiceResponse<string>
                    {
                        Message = "Hủy chỗ đặt thành công",
                        Success = true,
                        StatusCode = 200
                    };
                }

                return new ServiceResponse<string>
                {
                    Message = "Yêu cầu đã được duyệt, không thể hủy chỗ đặt",
                    StatusCode = 400,
                    Success = false,
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
*/