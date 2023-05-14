using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.CreateNewParkingSpotImage
{
    public class CreateNewParkingSpotImageCommandHandler : IRequestHandler<CreateNewParkingSpotImageCommand, ServiceResponse<int>>
    {
        private readonly IParkingSpotImageRepository _parkingSpotImageRepository;
        private readonly IParkingRepository _parkingRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public CreateNewParkingSpotImageCommandHandler(IParkingSpotImageRepository parkingSpotImageRepository, IParkingRepository parkingRepository)
        {
            _parkingSpotImageRepository = parkingSpotImageRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewParkingSpotImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkParkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(checkParkingExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var parkingSpotImageEntity = _mapper.Map<Domain.Entities.ParkingSpotImage>(request);
                await _parkingSpotImageRepository.Insert(parkingSpotImageEntity);
                return new ServiceResponse<int>
                {
                    Data = parkingSpotImageEntity.ParkingSpotImageId,
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
