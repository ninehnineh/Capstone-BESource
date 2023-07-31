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

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetListBookingFollowCalendar
{
    public class GetListBookingFollowCalendarQueryHandler : IRequestHandler<GetListBookingFollowCalendarQuery, ServiceResponse<IEnumerable<GetListBookingFollowCalendarResponse>>>
    {
        private readonly IBookingRepository _bookingRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetListBookingFollowCalendarQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListBookingFollowCalendarResponse>>> Handle(GetListBookingFollowCalendarQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var startDate = request.Start.Date;
                var endDate = request.End.Date;
                var lst = await _bookingRepository.GetListBookingFollowCalendarMethod(startDate, endDate);
                if (!lst.Any())
                {
                    return new ServiceResponse<IEnumerable<GetListBookingFollowCalendarResponse>>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListBookingFollowCalendarResponse>>(lst);
                return new ServiceResponse<IEnumerable<GetListBookingFollowCalendarResponse>>
                {
                    Data = lstDto,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công",
                    Count = lstDto.Count()
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
