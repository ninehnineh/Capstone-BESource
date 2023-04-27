using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Commands.DeleteVehicleInfor
{
    public class DeleteVehicleInforCommandHandler : IRequestHandler<DeleteVehicleInforCommand, ServiceResponse<string>>
    {
        private readonly IVehicleInfoRepository _vehicleInfoRepository;

        public DeleteVehicleInforCommandHandler(IVehicleInfoRepository vehicleInfoRepository)
        {
            _vehicleInfoRepository = vehicleInfoRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeleteVehicleInforCommand request, CancellationToken cancellationToken)
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
                        StatusCode = 200
                    };
                }
                await _vehicleInfoRepository.Delete(checkExist);
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
