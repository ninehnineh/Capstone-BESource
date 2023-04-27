using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Queries.GetListVehicleInforByUserId
{
    public class GetListVehicleInforByUserIdQueryHandler : IRequestHandler<GetListVehicleInforByUserIdQuery, ServiceResponse<IEnumerable<GetListVehicleInforByUserIdResponse>>>
    {
        private readonly IVehicleInfoRepository _vehicleInfoRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListVehicleInforByUserIdQueryHandler(IVehicleInfoRepository vehicleInfoRepository)
        {
            _vehicleInfoRepository = vehicleInfoRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListVehicleInforByUserIdResponse>>> Handle(GetListVehicleInforByUserIdQuery request, CancellationToken cancellationToken)
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
                List<Expression<Func<VehicleInfor, object>>> includes = new List<Expression<Func<VehicleInfor, object>>>
                {
                    x => x.Traffic
                };
                var lst = await _vehicleInfoRepository.GetAllItemWithPagination(x => x.UserId == request.UserId, includes, null, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListVehicleInforByUserIdResponse>>(lst);
                if(lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListVehicleInforByUserIdResponse>>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                return new ServiceResponse<IEnumerable<GetListVehicleInforByUserIdResponse>>
                {
                    Data = lstDto,
                    Message = "Thành công.",
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
