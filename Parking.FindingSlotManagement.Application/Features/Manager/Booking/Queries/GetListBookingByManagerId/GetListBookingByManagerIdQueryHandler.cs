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

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetListBookingByManagerId
{
    public class GetListBookingByManagerIdQueryHandler : IRequestHandler<GetListBookingByManagerIdQuery, ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetListBookingByManagerIdQueryHandler(IBookingRepository bookingRepository, IBusinessProfileRepository businessProfileRepository)
        {
            _bookingRepository = bookingRepository;
            _businessProfileRepository = businessProfileRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>> Handle(GetListBookingByManagerIdQuery request, CancellationToken cancellationToken)
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
                var businessExist = await _businessProfileRepository.GetItemWithCondition(x => x.UserId == request.ManagerId);
                if (businessExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>
                    {
                        Message = "Không tìm thấy tài khoản doanh nghiệp.",
                        StatusCode = 200,
                        Success = true
                    };
                }

                var lstBooking = await _bookingRepository.GetListBookingByManagerIdMethod(businessExist.BusinessProfileId, request.PageNo, request.PageSize);
                if (lstBooking == null)
                {
                    return new ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>
                    {
                        Message = "Không tìm thấy.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListBookingByManagerIdResponse>>(lstBooking);
                return new ServiceResponse<IEnumerable<GetListBookingByManagerIdResponse>>
                {
                    Data = lstDto,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
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


