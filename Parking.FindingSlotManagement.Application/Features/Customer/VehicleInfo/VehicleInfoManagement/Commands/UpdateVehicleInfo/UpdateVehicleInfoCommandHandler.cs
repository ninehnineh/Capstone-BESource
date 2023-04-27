using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Commands.UpdateVehicleInfo
{
    public class UpdateVehicleInfoCommandHandler : IRequestHandler<UpdateVehicleInfoCommand, ServiceResponse<string>>
    {
        private readonly IVehicleInfoRepository _vehicleInfoRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITrafficRepository _trafficRepository;

        public UpdateVehicleInfoCommandHandler(IVehicleInfoRepository vehicleInfoRepository, IAccountRepository accountRepository, ITrafficRepository trafficRepository)
        {
            _vehicleInfoRepository = vehicleInfoRepository;
            _accountRepository = accountRepository;
            _trafficRepository = trafficRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateVehicleInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _vehicleInfoRepository.GetById(request.VehicleInforId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy thông tin phương tiện.",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                if(!string.IsNullOrEmpty(request.LicensePlate))
                {
                    checkExist.LicensePlate = request.LicensePlate;
                }
                if (!string.IsNullOrEmpty(request.VehicleName))
                {
                    checkExist.VehicleName = request.VehicleName;
                }
                if (!string.IsNullOrEmpty(request.Color))
                {
                    checkExist.Color = request.Color;
                }
                if (!string.IsNullOrEmpty(request.UserId.ToString()))
                {
                    var checkUserExist = await _accountRepository.GetById(request.UserId);
                    if (checkUserExist == null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Không tìm thấy tài khoản.",
                            Success = true,
                            StatusCode = 200,
                            Count = 0
                        };
                    }
                    checkExist.UserId = request.UserId;
                }
                if (!string.IsNullOrEmpty(request.TrafficId.ToString()))
                {
                    var checkTrafficExist = await _trafficRepository.GetById(request.TrafficId);
                    if (checkTrafficExist == null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Không tìm thấy phương tiện.",
                            Success = true,
                            StatusCode = 200,
                            Count = 0
                        };
                    }
                    checkExist.TrafficId = request.TrafficId;
                }
                await _vehicleInfoRepository.Update(checkExist);
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
