using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ParkingManagement.Queries.GetAllParkingForAdmin
{
    public class GetAllParkingForAdminQueryHandler : IRequestHandler<GetAllParkingForAdminQuery, ServiceResponse<IEnumerable<GetAllParkingForAdminResponse>>>
    {
        private readonly IParkingRepository _parkingRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetAllParkingForAdminQueryHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetAllParkingForAdminResponse>>> Handle(GetAllParkingForAdminQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PageNo <= 0)
                {
                    request.PageNo = 1;
                }
                if (request.PageSize <= 0)
                {
                    request.PageSize = 1;
                }
                var lst = await _parkingRepository.GetAllItemWithPagination(null, null, x => x.ParkingId, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetAllParkingForAdminResponse>>(lst);
                if (lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetAllParkingForAdminResponse>>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                return new ServiceResponse<IEnumerable<GetAllParkingForAdminResponse>>
                {
                    Data = lstDto,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công",
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
