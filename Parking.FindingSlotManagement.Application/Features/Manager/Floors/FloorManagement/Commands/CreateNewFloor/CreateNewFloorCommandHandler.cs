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

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.CreateNewFloor
{
    public class CreateNewFloorCommandHandler : IRequestHandler<CreateNewFloorCommand, ServiceResponse<int>>
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IParkingRepository _parkingRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewFloorCommandHandler(IFloorRepository floorRepository, IParkingRepository parkingRepository)
        {
            _floorRepository = floorRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewFloorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkParkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(checkParkingExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy bãi xe.",
                        StatusCode = 200,
                        Success = true,
                        Count = 0
                    };
                }
                var _mapper = config.CreateMapper();
                var floorEntity = _mapper.Map<Floor>(request);
                floorEntity.IsActive = true;
                await _floorRepository.Insert(floorEntity);
                return new ServiceResponse<int>
                {
                    Data = floorEntity.FloorId,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
