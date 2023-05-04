using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Queries.GetVehicleInforById
{
    public class GetVehicleInforByIdQueryHandler : IRequestHandler<GetVehicleInforByIdQuery, ServiceResponse<GetVehicleInforByIdResponse>>
    {
        private readonly IVehicleInfoRepository _vehicleInfoRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetVehicleInforByIdQueryHandler(IVehicleInfoRepository vehicleInfoRepository)
        {
            _vehicleInfoRepository = vehicleInfoRepository;
        }
        public async Task<ServiceResponse<GetVehicleInforByIdResponse>> Handle(GetVehicleInforByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicleInfor = await _vehicleInfoRepository.GetById(request.VehicleInforId);
                if(vehicleInfor == null)
                {
                    return new ServiceResponse<GetVehicleInforByIdResponse>
                    {
                        Message = "Không tìm thấy thông tin phương tiện.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var vehicleInforDto = _mapper.Map<GetVehicleInforByIdResponse>(vehicleInfor);
                return new ServiceResponse<GetVehicleInforByIdResponse>
                {
                    Data = vehicleInforDto,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
