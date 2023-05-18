using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Queries.GetListTimelineByParkingPriceId
{
    public class GetListTimelineByParkingPriceIdQueryHandler : IRequestHandler<GetListTimelineByParkingPriceIdQuery, ServiceResponse<IEnumerable<GetListTimelineByParkingPriceIdResponse>>>
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
        public async Task<ServiceResponse<IEnumerable<GetListTimelineByParkingPriceIdResponse>>> Handle(GetListTimelineByParkingPriceIdQuery request, CancellationToken cancellationToken)
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
                    return new ServiceResponse<IEnumerable<GetListTimelineByParkingPriceIdResponse>>
                    {
                        Message = "Không tìm thấy gói.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var lst = await _timelineRepository.GetAllItemWithPagination(x => x.ParkingPriceId == request.ParkingPriceId, null, x => x.TimeLineId, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListTimelineByParkingPriceIdResponse>>(lst);
                if(lst.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListTimelineByParkingPriceIdResponse>>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                return new ServiceResponse<IEnumerable<GetListTimelineByParkingPriceIdResponse>>
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
