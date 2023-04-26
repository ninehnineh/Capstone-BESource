using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetTraffic
{
    public class GetTrafficQueryHandler : IRequestHandler<GetTrafficQuery, ServiceResponse<GetTrafficResponse>>
    {
        private readonly ITrafficRepository _trafficRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetTrafficQueryHandler(ITrafficRepository trafficRepository)
        {
            _trafficRepository = trafficRepository;
        }
        public async Task<ServiceResponse<GetTrafficResponse>> Handle(GetTrafficQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var traffic = await _trafficRepository.GetById(request.TrafficId);
                var _mapper = config.CreateMapper();
                var trafficDto = _mapper.Map<GetTrafficResponse>(traffic);
                if(traffic == null)
                {
                    return new ServiceResponse<GetTrafficResponse>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                return new ServiceResponse<GetTrafficResponse>
                {
                    Data = trafficDto,
                    Success = true,
                    Message = "Thành công",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
