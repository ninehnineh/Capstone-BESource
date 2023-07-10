using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCard
{
    public class GetStatisticBaseOnCardQueryHandler : IRequestHandler<GetStatisticBaseOnCardQuery, ServiceResponse<GetStatisticBaseOnCardResponse>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        private readonly IParkingRepository _parkingRepository;

        public GetStatisticBaseOnCardQueryHandler(IBookingRepository bookingRepository, IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository, IParkingRepository parkingRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _businessProfileRepository = businessProfileRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<GetStatisticBaseOnCardResponse>> Handle(GetStatisticBaseOnCardQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalNumberOfOrders = 0;
                var totalOfRevenue = 0M;
                var totalNumberOfOrdersInCurrentDay = 0;
                var totalWaitingOrder = 0;
                var managerExist = await _userRepository.GetById(request.ManagerId);
                if (managerExist == null)
                {
                    return new ServiceResponse<GetStatisticBaseOnCardResponse>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if (managerExist.RoleId != 1)
                {
                    return new ServiceResponse<GetStatisticBaseOnCardResponse>
                    {
                        Message = "Tài khoản không phải là quản lý.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                var businessExist = await _businessProfileRepository.GetItemWithCondition(x => x.UserId == request.ManagerId);
                if (businessExist == null)
                {
                    return new ServiceResponse<GetStatisticBaseOnCardResponse>
                    {
                        Message = "Không tìm thấy thông tin doanh nghiệp.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                //get list parking
                var lstParking = await _parkingRepository.GetAllItemWithCondition(x => x.BusinessId == businessExist.BusinessProfileId);
                if (!lstParking.Any())
                {
                    return new ServiceResponse<GetStatisticBaseOnCardResponse>
                    {
                        Message = "Không tìm thấy thông tin bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                foreach (var item in lstParking)
                {
                    totalNumberOfOrders += await _bookingRepository.GetTotalOrdersByParkingIdMethod(item.ParkingId);
                    totalOfRevenue += await _bookingRepository.GetRevenueByParkingIdMethod(item.ParkingId);
                    totalNumberOfOrdersInCurrentDay += await _bookingRepository.GetTotalNumberOfOrdersInCurrentDayByParkingIdMethod(item.ParkingId);
                    totalWaitingOrder += await _bookingRepository.GetTotalWaitingOrdersByParkingIdMethod(item.ParkingId);
                }
                GetStatisticBaseOnCardResponse res = new()
                {
                    NumberOfOrders = totalNumberOfOrders,
                    TotalOfRevenue = totalOfRevenue,
                    NumberOfOrdersInCurrentDay = totalNumberOfOrdersInCurrentDay,
                    WaitingOrder = totalWaitingOrder
                };
                return new ServiceResponse<GetStatisticBaseOnCardResponse>
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
