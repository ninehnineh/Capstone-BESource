using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetBookingDetails
{
    public class GetBookingDetailsQueryHandler : IRequestHandler<GetBookingDetailsQuery, ServiceResponse<GetBookingDetailsResponse>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingDetailsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetBookingDetailsResponse>> Handle(GetBookingDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var includes = new List<Expression<Func<Domain.Entities.Booking, object>>>
                {
                    x => x.User,
                    x => x.VehicleInfor,
                    x => x.ParkingSlot,
                    x => x.ParkingSlot.Floor,
                };

                var booking = await _bookingRepository
                    .GetItemWithCondition(x => x.BookingId == request.Bookingid, includes);

                if (booking == null)
                {
                    return new ServiceResponse<GetBookingDetailsResponse>
                    {
                        Message = "Đơn đặt không tồn tại",
                        StatusCode = 200,
                        Success = true,
                    };
                }

                var response = new GetBookingDetailsResponse
                {
                    BookingDetails = _mapper.Map<BookingDetailsDto>(booking),
                    User = _mapper.Map<UserBookingDto>(booking.User),
                    VehicleInfor = _mapper.Map<VehicleInforDto>(booking.VehicleInfor),
                    ParkingSlot = _mapper.Map<BookedParkingSlotDto>(booking.ParkingSlot)
                };

                return new ServiceResponse<GetBookingDetailsResponse>
                {
                    Data = response,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
