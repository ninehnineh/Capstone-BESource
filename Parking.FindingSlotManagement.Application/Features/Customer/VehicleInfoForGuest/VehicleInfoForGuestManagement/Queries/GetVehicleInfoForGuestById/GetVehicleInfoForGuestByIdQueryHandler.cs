using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Queries.GetVehicleInfoForGuestById
{
    public class GetVehicleInfoForGuestByIdQueryHandler : IRequestHandler<GetVehicleInfoForGuestByIdQuery, ServiceResponse<GetVehicleInfoForGuestByIdResponse>>
    {
        private readonly IVehicleInfoRepository _vehicleInfoRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetVehicleInfoForGuestByIdQueryHandler(IVehicleInfoRepository vehicleInfoRepository)
        {
            _vehicleInfoRepository = vehicleInfoRepository;
        }
        public async Task<ServiceResponse<GetVehicleInfoForGuestByIdResponse>> Handle(GetVehicleInfoForGuestByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicleInfor = await _vehicleInfoRepository.GetById(request.VehicleInforId);
                if (vehicleInfor == null)
                {
                    return new ServiceResponse<GetVehicleInfoForGuestByIdResponse>
                    {
                        Message = "Không tìm thấy thông tin phương tiện.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var vehicleInforDto = _mapper.Map<GetVehicleInfoForGuestByIdResponse>(vehicleInfor);
                return new ServiceResponse<GetVehicleInfoForGuestByIdResponse>
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
