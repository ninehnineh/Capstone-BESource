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

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Commands.CreateVehicleInfoForGuest
{
    public class VehicleInfoForGuestHandler : IRequestHandler<VehicleInfoForGuestCommand, ServiceResponse<int>>
    {
        private readonly IVehicleInfoRepository _vehicleInfoRepository;
        private readonly ITrafficRepository _trafficRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public VehicleInfoForGuestHandler(IVehicleInfoRepository vehicleInfoRepository, ITrafficRepository trafficRepository)
        {
            _vehicleInfoRepository = vehicleInfoRepository;
            _trafficRepository = trafficRepository;
        }
        public async Task<ServiceResponse<int>> Handle(VehicleInfoForGuestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var checkTrafficExist = await _trafficRepository.GetById(request.TrafficId);
                if (checkTrafficExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy phương tiện.",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                var _mapper = config.CreateMapper();
                var vehicleInfoForGuestEntity = _mapper.Map<VehicleInfor>(request);
                await _vehicleInfoRepository.Insert(vehicleInfoForGuestEntity);
                return new ServiceResponse<int>
                {
                    Data = vehicleInfoForGuestEntity.VehicleInforId,
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
