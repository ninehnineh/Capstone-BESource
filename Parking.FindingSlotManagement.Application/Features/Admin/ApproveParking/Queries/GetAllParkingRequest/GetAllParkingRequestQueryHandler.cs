using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetAllParkingRequest
{
    public class GetAllParkingRequestQueryHandler : IRequestHandler<GetAllParkingRequestQuery, ServiceResponse<IEnumerable<GetAllParkingRequestResponse>>>
    {
        private readonly IParkingRepository _parkingRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetAllParkingRequestQueryHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetAllParkingRequestResponse>>> Handle(GetAllParkingRequestQuery request, CancellationToken cancellationToken)
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
                List<Expression<Func<Domain.Entities.Parking, object>>> includes = new List<Expression<Func<Domain.Entities.Parking, object>>>
                {
                    x => x.BusinessProfile,
                    x => x.ApproveParkings
                };
                var lst = await _parkingRepository.GetAllItemWithPagination(null, includes, x => x.ParkingId, true, request.PageNo, request.PageSize);
                if(!lst.Any())
                {
                    return new ServiceResponse<IEnumerable<GetAllParkingRequestResponse>>
                    {
                        Message = "Danh sách trống.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetAllParkingRequestResponse>>(lst);
                return new ServiceResponse<IEnumerable<GetAllParkingRequestResponse>>
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
