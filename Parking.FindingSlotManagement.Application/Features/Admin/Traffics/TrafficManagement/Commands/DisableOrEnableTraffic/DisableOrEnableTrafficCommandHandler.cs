using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.DisableOrEnableTraffic
{
    public class DisableOrEnableTrafficCommandHandler : IRequestHandler<DisableOrEnableTrafficCommand, ServiceResponse<string>>
    {
        private readonly ITrafficRepository _trafficRepository;

        public DisableOrEnableTrafficCommandHandler(ITrafficRepository trafficRepository)
        {
            _trafficRepository = trafficRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DisableOrEnableTrafficCommand request, CancellationToken cancellationToken)
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
                if(checkExist.IsActive == true)
                {
                    checkExist.IsActive = false;
                }
                else if(checkExist.IsActive == false)
                {
                    checkExist.IsActive = true;
                }
                await _trafficRepository.Save();
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
