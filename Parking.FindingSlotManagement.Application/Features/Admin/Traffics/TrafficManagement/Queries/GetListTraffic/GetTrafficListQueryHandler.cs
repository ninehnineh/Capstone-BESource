using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetListTraffic
{
    public class GetTrafficListQueryHandler : IRequestHandler<GetTrafficListQuery, ServiceResponse<IEnumerable<GetListTrafficResponse>>>
    {
        private readonly ITrafficRepository _trafficRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetTrafficListQueryHandler(ITrafficRepository trafficRepository)
        {
            _trafficRepository = trafficRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListTrafficResponse>>> Handle(GetTrafficListQuery request, CancellationToken cancellationToken)
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
                var lst = await _trafficRepository.GetAllItemWithPagination(null, null, x => x.TrafficId, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListTrafficResponse>>(lst);
                if(lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListTrafficResponse>>()
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "Không tìm thấy",
                        Count = 0
                    };
                }
                return new ServiceResponse<IEnumerable<GetListTrafficResponse>>
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
