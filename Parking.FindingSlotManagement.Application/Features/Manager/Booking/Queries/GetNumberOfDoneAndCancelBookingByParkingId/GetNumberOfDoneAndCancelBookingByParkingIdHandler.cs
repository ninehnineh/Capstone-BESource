using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetNumberOfDoneAndCancelBookingByParkingId
{
    public class GetNumberOfDoneAndCancelBookingByParkingIdHandler : IRequestHandler<GetNumberOfDoneAndCancelBookingByParkingIdQuery, ServiceResponse<GetNumberOfDoneAndCancelBookingByParkingIdRes>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkingRepository _parkingRepository;

        public GetNumberOfDoneAndCancelBookingByParkingIdHandler(IBookingRepository bookingRepository,  IParkingRepository parkingRepository)
        {
            _bookingRepository = bookingRepository;
            _parkingRepository = parkingRepository;
        }

        public async Task<ServiceResponse<GetNumberOfDoneAndCancelBookingByParkingIdRes>> Handle(GetNumberOfDoneAndCancelBookingByParkingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalDoneBooking = 0;
                var totalCancelBooking = 0;
                var parkingExist = await _parkingRepository.GetItemWithCondition(x => x.ParkingId == request.ParkingId);
                if (parkingExist == null)
                {
                    return new ServiceResponse<GetNumberOfDoneAndCancelBookingByParkingIdRes>
                    {
                        Message = "Không tìm thấy thông tin bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }

                var done = await _bookingRepository.GetListBookingDoneOrCancelByParkingIdMethod(parkingExist.ParkingId, Domain.Enum.BookingStatus.Done.ToString());
                if (done != 0)
                {
                    totalDoneBooking += done;
                }
                var cancel = await _bookingRepository.GetListBookingDoneOrCancelByParkingIdMethod(parkingExist.ParkingId, Domain.Enum.BookingStatus.Cancel.ToString());
                if (cancel != 0)
                {
                    totalCancelBooking += cancel;
                }
                GetNumberOfDoneAndCancelBookingByParkingIdRes res = new GetNumberOfDoneAndCancelBookingByParkingIdRes
                {
                    NumberOfDoneBooking = totalDoneBooking,
                    NumberOfCancelBooking = totalCancelBooking,
                    Total = totalDoneBooking + totalCancelBooking
                };
                return new ServiceResponse<GetNumberOfDoneAndCancelBookingByParkingIdRes>
                {
                    Success = true,
                    Message = "Thành công",
                    StatusCode = 200,
                    Data = res
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
