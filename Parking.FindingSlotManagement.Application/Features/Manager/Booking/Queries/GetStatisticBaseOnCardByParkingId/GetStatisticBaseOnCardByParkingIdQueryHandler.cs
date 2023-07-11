using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCardByParkingId
{
    public class GetStatisticBaseOnCardByParkingIdQueryHandler : IRequestHandler<GetStatisticBaseOnCardByParkingIdQuery, ServiceResponse<GetStatisticBaseOnCardByParkingIdResponse>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkingRepository _parkingRepository;

        public GetStatisticBaseOnCardByParkingIdQueryHandler(IBookingRepository bookingRepository, IParkingRepository parkingRepository)
        {
            _bookingRepository = bookingRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<GetStatisticBaseOnCardByParkingIdResponse>> Handle(GetStatisticBaseOnCardByParkingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalNumberOfOrders = 0;
                var totalOfRevenue = 0M;
                var totalNumberOfOrdersInCurrentDay = 0;
                var totalWaitingOrder = 0;
                var parkingExist = await _parkingRepository.GetItemWithCondition(x => x.ParkingId == request.ParkingId);
                if (parkingExist == null)
                {
                    return new ServiceResponse<GetStatisticBaseOnCardByParkingIdResponse>
                    {
                        Message = "Không tìm thấy thông tin bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }

                totalNumberOfOrders += await _bookingRepository.GetTotalOrdersByParkingIdMethod(parkingExist.ParkingId);
                totalOfRevenue += await _bookingRepository.GetRevenueByParkingIdMethod(parkingExist.ParkingId);
                totalNumberOfOrdersInCurrentDay += await _bookingRepository.GetTotalNumberOfOrdersInCurrentDayByParkingIdMethod(parkingExist.ParkingId);
                totalWaitingOrder += await _bookingRepository.GetTotalWaitingOrdersByParkingIdMethod(parkingExist.ParkingId);
                GetStatisticBaseOnCardByParkingIdResponse res = new()
                {
                    NumberOfOrders = totalNumberOfOrders,
                    TotalOfRevenue = totalOfRevenue,
                    NumberOfOrdersInCurrentDay = totalNumberOfOrdersInCurrentDay,
                    WaitingOrder = totalWaitingOrder
                };
                return new ServiceResponse<GetStatisticBaseOnCardByParkingIdResponse>
                {
                    Data = res,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
