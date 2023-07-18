using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllBookingByKeeperId;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Queries.FilterBookingForKeeper
{
    public class FilterBookingForKeeperQueryHandler : IRequestHandler<FilterBookingForKeeperQuery, ServiceResponse<IEnumerable<FilterBookingForKeeperResponse>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper _mapper;

        public FilterBookingForKeeperQueryHandler(IBookingRepository bookingRepository, IUserRepository userRepository, IParkingRepository parkingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _parkingRepository = parkingRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<FilterBookingForKeeperResponse>>> Handle(FilterBookingForKeeperQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var KeeperExist = await _userRepository.GetById(request.KeeperId);
                if (KeeperExist == null)
                {
                    return new ServiceResponse<IEnumerable<FilterBookingForKeeperResponse>>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                if (KeeperExist.RoleId != 2)
                {
                    return new ServiceResponse<IEnumerable<FilterBookingForKeeperResponse>>
                    {
                        Message = "Tài khoản không phải là nhân viên bãi xe.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var parkingExist = await _parkingRepository.GetById(KeeperExist.ParkingId);
                if (parkingExist == null)
                {
                    return new ServiceResponse<IEnumerable<FilterBookingForKeeperResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = false,
                        StatusCode = 404

                    };
                }
                if(request.Date.ToString() == null)
                {
                    request.Date = null;
                }
                if(request.Status == null)
                {
                    request.Status = null;
                }
                var lstBooking = await _bookingRepository.FilterBookingForKeeperMethod(parkingExist.ParkingId, request.Date, request.Status, request.PageNo, request.PageSize);
                if(lstBooking == null)
                {
                    return new ServiceResponse<IEnumerable<FilterBookingForKeeperResponse>>
                    {
                        Message = "Không tìm thấy đơn.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                List<FilterBookingForKeeperResponse> lstReturn = new();
                foreach (var booking in lstBooking)
                {
                    var eachEntity = new FilterBookingForKeeperResponse
                    {
                        BookingSearchResult = _mapper.Map<BookingSearchResult>(booking),
                        VehicleInforSearchResult = _mapper.Map<VehicleInforSearchResult>(booking.VehicleInfor),
                        ParkingSearchResult = _mapper.Map<ParkingSearchResult>(parkingExist),
                        ParkingSlotSearchResult = _mapper.Map<ParkingSlotSearchResult>(booking.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot)
                    };
                    lstReturn.Add(eachEntity);
                }
                return new ServiceResponse<IEnumerable<FilterBookingForKeeperResponse>>
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
