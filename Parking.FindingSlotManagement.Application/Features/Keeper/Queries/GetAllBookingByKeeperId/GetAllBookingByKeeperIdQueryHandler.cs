using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllBookingByKeeperId
{
    public class GetAllBookingByKeeperIdQueryHandler : IRequestHandler<GetAllBookingByKeeperIdQuery, ServiceResponse<IEnumerable<GetAllBookingByKeeperIdResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper _mapper;

        public GetAllBookingByKeeperIdQueryHandler(IUserRepository userRepository, IBookingRepository bookingRepository, IParkingRepository parkingRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _parkingRepository = parkingRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<GetAllBookingByKeeperIdResponse>>> Handle(GetAllBookingByKeeperIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var KeeperExist = await _userRepository.GetById(request.KeeperId);
                if(KeeperExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetAllBookingByKeeperIdResponse>>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                if(KeeperExist.RoleId != 2)
                {
                    return new ServiceResponse<IEnumerable<GetAllBookingByKeeperIdResponse>>
                    {
                        Message = "Tài khoản không phải là nhân viên bãi xe.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var parkingExist = await _parkingRepository.GetById(KeeperExist.ParkingId);
                if (parkingExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetAllBookingByKeeperIdResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = false,
                        StatusCode = 404

                    };
                }
                var lstBooking = await _bookingRepository.GetAllBookingByParkingIdMethod(parkingExist.ParkingId, request.PageNo, request.PageSize);
                if (lstBooking == null)
                {
                    return new ServiceResponse<IEnumerable<GetAllBookingByKeeperIdResponse>>
                    {
                        Message = "Không tìm thấy đơn đặt",
                        Success = false,
                        StatusCode = 404
                    };
                }
                List<GetAllBookingByKeeperIdResponse> lstReturn = new();
                foreach (var booking in lstBooking)
                {
                    var eachEntity = new GetAllBookingByKeeperIdResponse
                    {
                        BookingSearchResult = _mapper.Map<BookingSearchResult>(booking),
                        VehicleInforSearchResult = _mapper.Map<VehicleInforSearchResult>(booking.VehicleInfor),
                        ParkingSearchResult = _mapper.Map<ParkingSearchResult>(parkingExist),
                        ParkingSlotSearchResult = _mapper.Map<ParkingSlotSearchResult>(booking.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot)
                    };
                    lstReturn.Add(eachEntity);
                }
                return new ServiceResponse<IEnumerable<GetAllBookingByKeeperIdResponse>>
                {
                    Data = lstReturn.OrderByDescending(x => x.BookingSearchResult.BookingId),
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200,
                    Count = lstReturn.Count()
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
