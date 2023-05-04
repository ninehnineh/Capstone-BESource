using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.UpdateTraffic
{
    public class UpdateTrafficCommandHandler : IRequestHandler<UpdateTrafficCommand, ServiceResponse<string>>
    {
        private readonly ITrafficRepository _trafficRepository;
        public UpdateTrafficCommandHandler(ITrafficRepository trafficRepository)
        {
            _trafficRepository = trafficRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateTrafficCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _trafficRepository.GetById(request.TrafficId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                if(!string.IsNullOrEmpty(request.Name))
                {
                    var checkNameExist = await _trafficRepository.GetItemWithCondition(x => x.Name.Equals(request.Name));
                    if(checkNameExist != null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Tên phương tiện đã tồn tại. Hãy nhập tên khác!!!",
                            StatusCode = 200,
                            Success= true
                        };
                    }
                    checkExist.Name = request.Name;
                }
                await _trafficRepository.Update(checkExist);
                return new ServiceResponse<string>
                {
                    StatusCode = 204,
                    Message = "Thành công",
                    Success = true,
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
