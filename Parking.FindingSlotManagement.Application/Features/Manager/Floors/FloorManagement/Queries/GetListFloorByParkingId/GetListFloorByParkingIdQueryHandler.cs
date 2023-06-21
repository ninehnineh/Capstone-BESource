using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloorByParkingId
{
    public class GetListFloorByParkingIdQueryHandler : IRequestHandler<GetListFloorByParkingIdQuery, ServiceResponse<IEnumerable<GetListFloorByParkingIdResponse>>>
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IParkingRepository _parkingRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListFloorByParkingIdQueryHandler(IFloorRepository floorRepository, IParkingRepository parkingRepository)
        {
            _floorRepository = floorRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListFloorByParkingIdResponse>>> Handle(GetListFloorByParkingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(parkingExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetListFloorByParkingIdResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var lstFloor = await _floorRepository.GetAllItemWithConditionByNoInclude(x => x.ParkingId == request.ParkingId && x.IsActive == true);
                if(lstFloor == null || lstFloor.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListFloorByParkingIdResponse>>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListFloorByParkingIdResponse>>(lstFloor);
                return new ServiceResponse<IEnumerable<GetListFloorByParkingIdResponse>>
                {
                    Data = lstDto,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200,
                    Count = lstFloor.Count()
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
