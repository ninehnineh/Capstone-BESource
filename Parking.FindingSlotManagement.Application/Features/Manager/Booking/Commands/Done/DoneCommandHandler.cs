using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.Done
{
    public class DoneCommandHandler : IRequestHandler<DoneCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;

        public DoneCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<ServiceResponse<string>> Handle(DoneCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingRepository
                    .GetItemWithCondition(x => x.BookingId == request.BookingId, null, false);

                if (booking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Đơn đặt không tồn tại",
                        StatusCode = 200,
                        Success = false
                    };
                }

                booking.Status = BookingStatus.Done.ToString();
                await _bookingRepository.Save();

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
