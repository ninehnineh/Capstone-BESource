using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetNumberOfDoneAndCancelBooking
{
    public class GetNumberOfDoneAndCancelBookingQueryHandler : IRequestHandler<GetNumberOfDoneAndCancelBookingQuery, ServiceResponse<GetNumberOfDoneAndCancelBookingRes>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        private readonly IParkingRepository _parkingRepository;

        public GetNumberOfDoneAndCancelBookingQueryHandler(IBookingRepository bookingRepository, IUserRepository userRepository, IBusinessProfileRepository businessProfileRepository, IParkingRepository parkingRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _businessProfileRepository = businessProfileRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<GetNumberOfDoneAndCancelBookingRes>> Handle(GetNumberOfDoneAndCancelBookingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalDoneBooking = 0;
                var totalCancelBooking = 0;
                var managerExist = await _userRepository.GetById(request.ManagerId);
                if(managerExist == null)
                {
                    return new ServiceResponse<GetNumberOfDoneAndCancelBookingRes>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if (managerExist.RoleId != 1)
                {
                    return new ServiceResponse<GetNumberOfDoneAndCancelBookingRes>
                    {
                        Message = "Tài khoản không phải là quản lý.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                var businessExist = await _businessProfileRepository.GetItemWithCondition(x => x.UserId == request.ManagerId);
                if(businessExist == null)
                {
                    return new ServiceResponse<GetNumberOfDoneAndCancelBookingRes>
                    {
                        Message = "Không tìm thấy thông tin doanh nghiệp.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                //get list parking
                var lstParking = await _parkingRepository.GetAllItemWithCondition(x => x.BusinessId == businessExist.BusinessProfileId);
                if(!lstParking.Any())
                {
                    return new ServiceResponse<GetNumberOfDoneAndCancelBookingRes>
                    {
                        Message = "Không tìm thấy thông tin bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                foreach (var item in lstParking)
                {
                    var done = await _bookingRepository.GetListBookingDoneOrCancelByParkingIdMethod(item.ParkingId, Domain.Enum.BookingStatus.Done.ToString());
                    if(done != 0)
                    {
                        totalDoneBooking += done;
                    }
                    var cancel = await _bookingRepository.GetListBookingDoneOrCancelByParkingIdMethod(item.ParkingId, Domain.Enum.BookingStatus.Cancel.ToString());
                    if (cancel != 0)
                    {
                        totalCancelBooking += cancel;
                    }
                }
                GetNumberOfDoneAndCancelBookingRes res = new GetNumberOfDoneAndCancelBookingRes
                {
                    NumberOfDoneBooking = totalDoneBooking,
                    NumberOfCancelBooking = totalCancelBooking,
                    Total = totalDoneBooking + totalCancelBooking
                };
                return new ServiceResponse<GetNumberOfDoneAndCancelBookingRes>
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
