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

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Commands.CreateNewVehicleInfo
{
    public class VehicleInfoCommandHandler : IRequestHandler<VehicleInfoCommand, ServiceResponse<int>>
    {
        private readonly IVehicleInfoRepository _vehicleInfoRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITrafficRepository _trafficRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public VehicleInfoCommandHandler(IVehicleInfoRepository vehicleInfoRepository, IAccountRepository accountRepository, ITrafficRepository trafficRepository)
        {
            _vehicleInfoRepository = vehicleInfoRepository;
            _accountRepository = accountRepository;
            _trafficRepository = trafficRepository;
        }
        public async Task<ServiceResponse<int>> Handle(VehicleInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _accountRepository.GetById(request.UserId);
                if(checkUserExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success =true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
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
                var vehicleInforEntity = _mapper.Map<VehicleInfor>(request);
                await _vehicleInfoRepository.Insert(vehicleInforEntity);
                return new ServiceResponse<int>
                {
                    Data = vehicleInforEntity.VehicleInforId,
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
