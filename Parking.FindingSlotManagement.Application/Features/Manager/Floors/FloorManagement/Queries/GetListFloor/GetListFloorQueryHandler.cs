using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloor
{
    public class GetListFloorQueryHandler : IRequestHandler<GetListFloorQuery, ServiceResponse<IEnumerable<GetListFloorResponse>>>
    {
        private readonly IFloorRepository _floorRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListFloorQueryHandler(IFloorRepository floorRepository)
        {
            _floorRepository = floorRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListFloorResponse>>> Handle(GetListFloorQuery request, CancellationToken cancellationToken)
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
                var lst = await _floorRepository.GetAllItemWithPagination(x => x.IsActive == true, null, null, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListFloorResponse>>(lst);
                if (lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListFloorResponse>>
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = "Không tìm thấy",
                        Count = 0
                    };
                }
                return new ServiceResponse<IEnumerable<GetListFloorResponse>>
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
