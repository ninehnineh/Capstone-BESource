using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetParkingById
{
    public class GetParkingByIdQueryHandler : IRequestHandler<GetParkingByIdQuery, ServiceResponse<GetParkingByIdResponse>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetParkingByIdQueryHandler(IParkingRepository parkingRepository, IFloorRepository floorRepository, IParkingHasPriceRepository parkingHasPriceRepository)
        {
            _parkingRepository = parkingRepository;
            _floorRepository = floorRepository;
            _parkingHasPriceRepository = parkingHasPriceRepository;
        }
        public async Task<ServiceResponse<GetParkingByIdResponse>> Handle(GetParkingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                int parkingHasPriceCount;
                int floorCount;
                var parkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(parkingExist == null)
                {
                    return new ServiceResponse<GetParkingByIdResponse>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                /*if(parkingExist.IsActive == false)
                {
                    return new ServiceResponse<GetParkingByIdResponse>
                    {
                        Message = "Bãi đã bị cấm.",
                        StatusCode = 400,
                        Success = true
                    };
                }*/
                var _mapper = config.CreateMapper();
                var parkingEntity = _mapper.Map<ParkingEntity>(parkingExist);

                var floorExist = await _floorRepository.GetAllItemWithConditionByNoInclude(x => x.ParkingId == request.ParkingId && x.IsActive == true);

                if (floorExist == null || floorExist.Count() == 0)
                {
                    floorCount = 0;
                }
                else
                {
                    floorCount = floorExist.Count();
                }
                var parkingHasPriceExist = await _parkingHasPriceRepository.GetAllItemWithConditionByNoInclude(x => x.ParkingId == request.ParkingId);
                
                if (parkingHasPriceExist == null || parkingHasPriceExist.Count() == 0)
                {
                    parkingHasPriceCount = 0;
                }
                else
                {
                    parkingHasPriceCount = parkingHasPriceExist.Count();

                }
                var res = new GetParkingByIdResponse
                {
                    ParkingEntity = parkingEntity,
                    NumberOfFloors = floorCount,
                    NumberofParkingHasPrice = parkingHasPriceCount
                };
                return new ServiceResponse<GetParkingByIdResponse>
                {
                    Data = res,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
                    Count = 1
                };


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
