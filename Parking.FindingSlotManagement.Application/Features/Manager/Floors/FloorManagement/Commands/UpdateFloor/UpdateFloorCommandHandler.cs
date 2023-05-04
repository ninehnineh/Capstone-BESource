using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.UpdateFloor
{
    public class UpdateFloorCommandHandler : IRequestHandler<UpdateFloorCommand, ServiceResponse<string>>
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IParkingRepository _parkingRepository;

        public UpdateFloorCommandHandler(IFloorRepository floorRepository, IParkingRepository parkingRepository)
        {
            _floorRepository = floorRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateFloorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkFloorExist = await _floorRepository.GetById(request.FloorId);
                if(checkFloorExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tầng.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(!string.IsNullOrEmpty(request.FloorName))
                {
                    checkFloorExist.FloorName = request.FloorName;
                }
                if(!string.IsNullOrEmpty(request.ParkingId.ToString()))
                {
                    var checkParkingExist = await _parkingRepository.GetById(request.ParkingId);
                    if (checkParkingExist == null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Không tìm thấy bãi xe.",
                            StatusCode = 200,
                            Success = true,
                            Count = 0
                        };
                    }
                    checkFloorExist.ParkingId = request.ParkingId;
                }
                await _floorRepository.Update(checkFloorExist);
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
