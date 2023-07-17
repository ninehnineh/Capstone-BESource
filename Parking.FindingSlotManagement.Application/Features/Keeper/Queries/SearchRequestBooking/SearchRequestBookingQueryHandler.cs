using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
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
        private readonly IMapper _mapper;
        private readonly IParkingRepository _parkingRepository;

        public SearchRequestBookingQueryHandler(IBookingRepository bookingRepository, IMapper mapper, IParkingRepository parkingRepository)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<SearchRequestBookingResponse>>> Handle(SearchRequestBookingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(parkingExist == null)
                {
                    return new ServiceResponse<IEnumerable<SearchRequestBookingResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = false,
                        StatusCode = 404

                    };
                }
                var lstBooking = await _bookingRepository.SearchRequestBookingMethod(request.ParkingId, request.SearchString);
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
