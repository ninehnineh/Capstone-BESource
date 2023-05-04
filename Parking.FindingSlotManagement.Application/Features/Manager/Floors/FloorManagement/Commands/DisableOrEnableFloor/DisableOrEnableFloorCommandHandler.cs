using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.DisableOrEnableFloor
{
    public class DisableOrEnableFloorCommandHandler : IRequestHandler<DisableOrEnableFloorCommand, ServiceResponse<string>>
    {
        private readonly IFloorRepository _floorRepository;

        public DisableOrEnableFloorCommandHandler(IFloorRepository floorRepository)
        {
            _floorRepository = floorRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DisableOrEnableFloorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _floorRepository.GetById(request.FloorId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tầng.", 
                        StatusCode = 200,
                        Success = true
                    };
                }
                if(checkExist.IsActive == false)
                {
                    checkExist.IsActive = true;
                }
                else if(checkExist.IsActive == true)
                {
                    checkExist.IsActive = false;
                }
                await _floorRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
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
