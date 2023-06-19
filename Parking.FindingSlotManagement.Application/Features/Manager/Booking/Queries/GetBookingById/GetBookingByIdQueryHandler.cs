using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, ServiceResponse<GetBookingByIdResponse>>
    {
        private readonly IBookingRepository _bookingRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetBookingByIdQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        public async Task<ServiceResponse<GetBookingByIdResponse>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<Domain.Entities.Booking, object>>> includes = new List<Expression<Func<Domain.Entities.Booking, object>>>
                {
                    x => x.User,
                    x => x.ParkingSlot,
                    x => x.VehicleInfor,
                    x => x.VehicleInfor.Traffic,
                    x => x.ParkingSlot.Floor,
                    x => x.ParkingSlot.Floor.Parking
                };
                var bookingExist = await _bookingRepository.GetItemWithCondition(x => x.BookingId == request.BookingId, includes, true);
                if(bookingExist == null)
                {
                    return new ServiceResponse<GetBookingByIdResponse>
                    {
                        Message = "Không tìm thấy.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var _mapper = config.CreateMapper();
                var entityDto = _mapper.Map<GetBookingByIdResponse>(bookingExist);
                return new ServiceResponse<GetBookingByIdResponse>
                {
                    Data = entityDto,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
