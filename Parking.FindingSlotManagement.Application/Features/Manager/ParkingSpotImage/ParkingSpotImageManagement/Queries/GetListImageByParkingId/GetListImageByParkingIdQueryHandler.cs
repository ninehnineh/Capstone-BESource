using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Queries.GetListImageByParkingId
{
    public class GetListImageByParkingIdQueryHandler : IRequestHandler<GetListImageByParkingIdQuery, ServiceResponse<IEnumerable<GetListImageByParkingIdResponse>>>
    {
        private readonly IParkingSpotImageRepository _parkingSpotImageRepository;
        private readonly IParkingRepository _parkingRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListImageByParkingIdQueryHandler(IParkingSpotImageRepository parkingSpotImageRepository, IParkingRepository parkingRepository)
        {
            _parkingSpotImageRepository = parkingSpotImageRepository;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListImageByParkingIdResponse>>> Handle(GetListImageByParkingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var checkParkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(checkParkingExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetListImageByParkingIdResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Count = 0,
                        StatusCode = 200,
                        Success = true
                    };
                }
                var lst = await _parkingSpotImageRepository.GetAllItemWithPagination(x => x.ParkingId == request.ParkingId, null, null, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListImageByParkingIdResponse>>(lst);
                if(lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListImageByParkingIdResponse>>
                    {
                        Message = "Không tìm thấy.",
                        StatusCode = 200,
                        Success = true,
                    };
                }
                return new ServiceResponse<IEnumerable<GetListImageByParkingIdResponse>>
                {
                    Data = lstDto,
                    Count = lstDto.Count(),
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
