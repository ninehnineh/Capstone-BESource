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

namespace Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetListBusinessProfile
{
    public class GetListBusinessProfileQueryHandler : IRequestHandler<GetListBusinessProfileQuery, ServiceResponse<IEnumerable<GetListBusinessProfileResponse>>>
    {
        private readonly IBusinessProfileRepository _businessProfileRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListBusinessProfileQueryHandler(IBusinessProfileRepository businessProfileRepository)
        {
            _businessProfileRepository = businessProfileRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListBusinessProfileResponse>>> Handle(GetListBusinessProfileQuery request, CancellationToken cancellationToken)
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
                List<Expression<Func<Domain.Entities.BusinessProfile, object>>> includes = new List<Expression<Func<Domain.Entities.BusinessProfile, object>>>
                {
                    x => x.User
                };
                var lst = await _businessProfileRepository.GetAllItemWithPagination(null, includes, x => x.BusinessProfileId, true, request.PageNo, request.PageSize);
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListBusinessProfileResponse>>(lst);
                if(lstDto.Count() <= 0)
                {
                    return new ServiceResponse<IEnumerable<GetListBusinessProfileResponse>>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                return new ServiceResponse<IEnumerable<GetListBusinessProfileResponse>>
                {
                    Data = lstDto,
                    Message = "Thành công",
                    Success= true,
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
