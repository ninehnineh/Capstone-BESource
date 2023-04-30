using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Queries.GetBusinessProfileByUserId
{
    public class GetBusinessProfileByUserIdQueryHandler : IRequestHandler<GetBusinessProfileByUserIdQuery, ServiceResponse<GetBusinessProfileResponse>>
    {
        private readonly IBusinessProfileRepository _businessProfileRepository;
        private readonly IAccountRepository _accountRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetBusinessProfileByUserIdQueryHandler(IBusinessProfileRepository businessProfileRepository, IAccountRepository accountRepository)
        {
            _businessProfileRepository = businessProfileRepository;
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<GetBusinessProfileResponse>> Handle(GetBusinessProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _accountRepository.GetById(request.UserId);
                if (checkUserExist == null)
                {
                    return new ServiceResponse<GetBusinessProfileResponse>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var res = await _businessProfileRepository.GetItemWithCondition(x => x.UserId == request.UserId, null, true);
                if(res == null)
                {
                    return new ServiceResponse<GetBusinessProfileResponse>
                    {
                        Message = "Không tìm thấy thông tin doanh nghiệp.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var resDto = _mapper.Map<GetBusinessProfileResponse>(res);
                return new ServiceResponse<GetBusinessProfileResponse>
                {
                    Data = resDto,
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
