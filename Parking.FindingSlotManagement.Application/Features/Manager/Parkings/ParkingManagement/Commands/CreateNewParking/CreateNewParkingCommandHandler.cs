using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CreateNewParking
{
    public class CreateNewParkingCommandHandler : IRequestHandler<CreateNewParkingCommand, ServiceResponse<int>>
    {
        private readonly IParkingRepository _parkingRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewParkingCommandHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewParkingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _parkingRepository.GetItemWithCondition(x => x.Name.Equals(request.Name), null, true);
                if(checkExist != null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Tên bãi xe đã tồn tại. Vui lòng nhập tên bãi xe khác.",
                        StatusCode = 400,
                        Success = false,
                        Count = 0
                    };
                }
                var _mapper = config.CreateMapper();
                var parkingEntity = _mapper.Map<Domain.Entities.Parking>(request);
                parkingEntity.IsActive = true;
                parkingEntity.IsFull = false;
                await _parkingRepository.Insert(parkingEntity);
                parkingEntity.Code = "BX" + parkingEntity.ParkingId;
                await _parkingRepository.Save();
                return new ServiceResponse<int>
                {
                    Data = parkingEntity.ParkingId,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 201,
                    Count = 0
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
