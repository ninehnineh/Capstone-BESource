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

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Queries.GetListTimelineByParkingPriceId
{
    public class GetListTimelineByParkingPriceIdQueryHandler : IRequestHandler<GetListTimelineByParkingPriceIdQuery, ServiceResponse<GetListTimelineByParkingPriceIdResponse>>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IParkingPriceRepository _parkingPriceRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListTimelineByParkingPriceIdQueryHandler(ITimelineRepository timelineRepository, IParkingPriceRepository parkingPriceRepository)
        {
            _timelineRepository = timelineRepository;
            _parkingPriceRepository = parkingPriceRepository;
        }
        public async Task<ServiceResponse<GetListTimelineByParkingPriceIdResponse>> Handle(GetListTimelineByParkingPriceIdQuery request, CancellationToken cancellationToken)
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
                var parkingPriceExist = await _parkingPriceRepository.GetById(request.ParkingPriceId);
                if(parkingPriceExist == null)
                {
                    return new ServiceResponse<GetListTimelineByParkingPriceIdResponse>
                    {
                        Message = "Không tìm thấy gói.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                List<Expression<Func<Domain.Entities.ParkingPrice, object>>> includes = new()
                {
                    x => x.TimeLines
                };
                var lst = await _parkingPriceRepository.GetAllItemWithPagination(x => x.ParkingPriceId == request.ParkingPriceId, includes, null, true, request.PageNo, request.PageSize);
                
                if(lst.Count() <= 0)
                {
                    return new ServiceResponse<GetListTimelineByParkingPriceIdResponse>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var lstDto = new GetListTimelineByParkingPriceIdResponse()
                {
                    ParkingPriceRes = _mapper.Map<ParkingPriceRes>(lst.FirstOrDefault()),
                    LstTimeLineRes = _mapper.Map<List<TimeLineRes>>(lst.FirstOrDefault().TimeLines)
                };
                return new ServiceResponse<GetListTimelineByParkingPriceIdResponse>
                {
                    Data = lstDto,
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
