using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.CreateNewTraffic
{
    public class CreateNewTrafficCommandHandler : IRequestHandler<CreateNewTrafficCommand, ServiceResponse<int>>
    {
        private readonly ITrafficRepository _trafficRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewTrafficCommandHandler(ITrafficRepository trafficRepository)
        {
            _trafficRepository = trafficRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewTrafficCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _trafficRepository.GetItemWithCondition(x => x.Name.Equals(request.Name), null, true);
                if(checkExist != null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Tên phương tiện đã tồn tại. Vui lòng nhập lại tên phương tiện!!!",
                        StatusCode = 400,
                        Success = false,
                        Count = 0
                    };
                }
                var _mapper = config.CreateMapper();
                var trafficEntity = _mapper.Map<Traffic>(request);
                trafficEntity.IsActive = true;
                await _trafficRepository.Insert(trafficEntity);
                return new ServiceResponse<int>
                {
                    Data = trafficEntity.TrafficId,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 201,
                    Count = 0
                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
