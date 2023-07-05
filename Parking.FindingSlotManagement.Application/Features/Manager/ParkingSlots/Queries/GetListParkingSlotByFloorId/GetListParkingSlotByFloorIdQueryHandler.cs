using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetListParkingSlotByFloorId
{
    public class GetListParkingSlotByFloorIdQueryHandler : IRequestHandler<GetListParkingSlotByFloorIdQuery, ServiceResponse<IEnumerable<GetListParkingSlotByFloorIdResponse>>>
    {
        private readonly IParkingSlotRepository _parkingSlotRepository;
        private readonly IFloorRepository _floorRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetListParkingSlotByFloorIdQueryHandler(IParkingSlotRepository parkingSlotRepository, IFloorRepository floorRepository)
        {
            _parkingSlotRepository = parkingSlotRepository;
            _floorRepository = floorRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListParkingSlotByFloorIdResponse>>> Handle(GetListParkingSlotByFloorIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var floorExist = await _floorRepository.GetById(request.FloorId);
                if(floorExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetListParkingSlotByFloorIdResponse>>
                    {
                        Message = "Không tìm thấy tầng.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var lstParkingSlot = await _parkingSlotRepository.GetAllItemWithConditionByNoInclude(x => x.FloorId == request.FloorId);
                if(lstParkingSlot == null || lstParkingSlot.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListParkingSlotByFloorIdResponse>>
                    {
                        Message = "Không tìm thấy slot.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListParkingSlotByFloorIdResponse>>(lstParkingSlot);
                return new ServiceResponse<IEnumerable<GetListParkingSlotByFloorIdResponse>>
                {
                    Data = lstDto,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200,
                    Count = lstDto.Count()
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
