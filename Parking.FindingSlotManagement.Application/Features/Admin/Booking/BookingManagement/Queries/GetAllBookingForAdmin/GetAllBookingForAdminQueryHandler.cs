using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Booking.BookingManagement.Queries.GetAllBookingForAdmin
{
    public class GetAllBookingForAdminQueryHandler : IRequestHandler<GetAllBookingForAdminQuery, ServiceResponse<IEnumerable<GetAllBookingForAdminResponse>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetAllBookingForAdminQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<GetAllBookingForAdminResponse>>> Handle(GetAllBookingForAdminQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PageNo <= 0)
                {
                    request.PageNo = 1;
                }
                if (request.PageSize <= 0)
                {
                    request.PageSize = 1;
                }
                var lst = await _bookingRepository.GetAllBookingForAdminMethod(request.PageNo, request.PageSize);
                if(lst == null)
                {
                    return new ServiceResponse<IEnumerable<GetAllBookingForAdminResponse>>
                    {
                        Message = "Không tìm thấy.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                List<GetAllBookingForAdminResponse> resReturn = new();
                foreach (var item in lst)
                {
                    GetAllBookingForAdminResponse x = new GetAllBookingForAdminResponse()
                    {
                        BookingDtoForAdmin = _mapper.Map<BookingDtoForAdmin>(item),
                        ParkingDtoForAdmin = _mapper.Map<ParkingDtoForAdmin>(item.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.Parking),
                        FloorDtoForAdmin = _mapper.Map<FloorDtoForAdmin>(item.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor),
                        SlotDtoForAdmin = _mapper.Map<SlotDtoForAdmin>(item.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot),
                        UserForGetAllBookingForAdminResponse = _mapper.Map<UserForGetAllBookingForAdminResponse>(item.User)
                    };
                    resReturn.Add(x);
                }
                return new ServiceResponse<IEnumerable<GetAllBookingForAdminResponse>>
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
