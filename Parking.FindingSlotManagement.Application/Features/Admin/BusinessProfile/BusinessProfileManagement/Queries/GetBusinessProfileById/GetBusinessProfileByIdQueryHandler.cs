using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetBusinessProfileById
{
    public class GetBusinessProfileByIdQueryHandler : IRequestHandler<GetBusinessProfileByIdQuery, ServiceResponse<GetBusinessProfileByIdResponse>>
    {
        private readonly IBusinessProfileRepository _businessProfileRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetBusinessProfileByIdQueryHandler(IBusinessProfileRepository businessProfileRepository)
        {
            _businessProfileRepository = businessProfileRepository;
        }
        public async Task<ServiceResponse<GetBusinessProfileByIdResponse>> Handle(GetBusinessProfileByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _businessProfileRepository.GetById(request.BusinessProfileId);
                if(res == null)
                {
                    return new ServiceResponse<GetBusinessProfileByIdResponse>
                    {
                        Message = "Không tìm thấy thông tin doanh nghiệp",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var _mapper = config.CreateMapper();
                var resDto = _mapper.Map<GetBusinessProfileByIdResponse>(res);
                return new ServiceResponse<GetBusinessProfileByIdResponse>
                {
                    Data = resDto,
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
