using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetUpcommingBooking
{
    public class GetUpcommingBookingQueryHandler : IRequestHandler<GetUpcommingBookingQuery, ServiceResponse<IEnumerable<GetUpcommingBookingResponse>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUpcommingBookingQueryHandler(IBookingRepository bookingRepository, IUserRepository userRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<GetUpcommingBookingResponse>>> Handle(GetUpcommingBookingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _userRepository.GetById(request.UserId);
                if (checkUserExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetUpcommingBookingResponse>>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var lstBooking = await _bookingRepository.GetUpcommingBookingByUserIdMethod(request.UserId);
                if(lstBooking == null)
                {
                    return new ServiceResponse<IEnumerable<GetUpcommingBookingResponse>>
                    {
                        Message = "Không tìm thấy đơn đặt.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                List<GetUpcommingBookingResponse> lstReturn = new();
                foreach (var booking in lstBooking)
                {
                    var eachEntity = new GetUpcommingBookingResponse
                    {
                        BookingSearchResult = _mapper.Map<BookingSearchResult>(booking),
                        VehicleInforSearchResult = _mapper.Map<VehicleInforSearchResult>(booking.VehicleInfor),
                        ParkingSearchResult = _mapper.Map<ParkingSearchResult>(booking.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.Parking),
                        ParkingSlotSearchResult = _mapper.Map<ParkingSlotSearchResult>(booking.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot)
                    };
                    lstReturn.Add(eachEntity);
                }
                return new ServiceResponse<IEnumerable<GetUpcommingBookingResponse>>
                {
                    Data = lstReturn.OrderByDescending(x => x.BookingSearchResult.BookingId),
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công"
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
