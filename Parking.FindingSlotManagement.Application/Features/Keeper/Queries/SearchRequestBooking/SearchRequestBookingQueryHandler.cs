using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllBookingByKeeperId;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking
{
    public class SearchRequestBookingQueryHandler : IRequestHandler<SearchRequestBookingQuery, ServiceResponse<IEnumerable<SearchRequestBookingResponse>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IParkingRepository _parkingRepository;

        public SearchRequestBookingQueryHandler(IBookingRepository bookingRepository, IUserRepository userRepository, IMapper mapper, IParkingRepository parkingRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<SearchRequestBookingResponse>>> Handle(SearchRequestBookingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var KeeperExist = await _userRepository.GetById(request.KeeperId);
                if (KeeperExist == null)
                {
                    return new ServiceResponse<IEnumerable<SearchRequestBookingResponse>>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                if (KeeperExist.RoleId != 2)
                {
                    return new ServiceResponse<IEnumerable<SearchRequestBookingResponse>>
                    {
                        Message = "Tài khoản không phải là nhân viên bãi xe.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var parkingExist = await _parkingRepository.GetById(KeeperExist.ParkingId);
                if (parkingExist == null)
                {
                    return new ServiceResponse<IEnumerable<SearchRequestBookingResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = false,
                        StatusCode = 404

                    };
                }
                var lstBooking = await _bookingRepository.SearchRequestBookingMethod(parkingExist.ParkingId, request.SearchString);
                if(lstBooking == null)
                {
                    return new ServiceResponse<IEnumerable<SearchRequestBookingResponse>>
                    {
                        Message = "Không tìm thấy đơn đặt",
                        Success = false,
                        StatusCode = 404
                    };
                }
                List<SearchRequestBookingResponse> lstReturn = new();
                foreach(var booking in lstBooking)
                {
                    var eachEntity = new SearchRequestBookingResponse
                    {
                        BookingSearchResult = _mapper.Map<BookingSearchResult>(booking),
                        VehicleInforSearchResult = _mapper.Map<VehicleInforSearchResult>(booking.VehicleInfor),
                        ParkingSearchResult = _mapper.Map<ParkingSearchResult>(parkingExist),
                        ParkingSlotSearchResult = _mapper.Map<ParkingSlotSearchResult>(booking.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot)
                    };
                    lstReturn.Add(eachEntity);
                }
                return new ServiceResponse<IEnumerable<SearchRequestBookingResponse>>
                {
                    Data = lstReturn.OrderByDescending(x => x.BookingSearchResult.BookingId),
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
