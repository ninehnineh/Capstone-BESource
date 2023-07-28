using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Booking.BookingManagement.Queries.GetAllBookingForAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetAllBookingByParkingId
{
    public class GetAllBookingByParkingIdQueryHandler : IRequestHandler<GetAllBookingByParkingIdQuery, ServiceResponse<IEnumerable<GetAllBookingByParkingIdResponse>>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetAllBookingByParkingIdQueryHandler(IParkingRepository parkingRepository, IBookingRepository bookingRepository, IMapper mapper)
        {
            _parkingRepository = parkingRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<GetAllBookingByParkingIdResponse>>> Handle(GetAllBookingByParkingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(parkingExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetAllBookingByParkingIdResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                if (request.PageNo <= 0)
                {
                    request.PageNo = 1;
                }
                if (request.PageSize <= 0)
                {
                    request.PageSize = 1;
                }
                var lst = await _bookingRepository.GetAllBookingByParkingIdVer2Method(request.ParkingId, request.PageNo, request.PageSize);
                if(lst == null)
                {
                    return new ServiceResponse<IEnumerable<GetAllBookingByParkingIdResponse>>
                    {
                        Message = "Không tìm thấy.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                List<GetAllBookingByParkingIdResponse> resReturn = new();
                foreach (var item in lst)
                {
                    GetAllBookingByParkingIdResponse x = new()
                    {
                        BookingForGetAllBookingByParkingIdResponse = _mapper.Map<BookingForGetAllBookingByParkingIdResponse>(item),
                        FloorDtoForAdmin = _mapper.Map<FloorDtoForAdmin>(item.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor),
                        ParkingDtoForAdmin = _mapper.Map<ParkingDtoForAdmin>(item.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.Parking),
                        SlotDtoForAdmin = _mapper.Map<SlotDtoForAdmin>(item.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot),
                        UserForGetAllBookingByParkingIdResponse = _mapper.Map<UserForGetAllBookingByParkingIdResponse>(item.User),
                        VehicleForGetAllBookingByParkingIdResponse = _mapper.Map<VehicleForGetAllBookingByParkingIdResponse>(item.VehicleInfor)
                    };
                    resReturn.Add(x);
                }
                return new ServiceResponse<IEnumerable<GetAllBookingByParkingIdResponse>>
                {
                    Data = resReturn,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200,
                    Count = resReturn.Count()
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
