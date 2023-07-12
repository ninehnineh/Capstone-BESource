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

namespace Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Queries.GetInforOfBusinessByManagerId
{
    public class GetInforOfBusinessByManagerIdQueryHandler : IRequestHandler<GetInforOfBusinessByManagerIdQuery, ServiceResponse<GetInforOfBusinessByManagerIdResponse>>
    {
        private readonly IUserRepository _userRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetInforOfBusinessByManagerIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<GetInforOfBusinessByManagerIdResponse>> Handle(GetInforOfBusinessByManagerIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<User, object>>> includes = new()
                {
                    x => x.Role,
                    x => x.BusinessProfile
                };
                var managerExist = await _userRepository.GetItemWithCondition(x => x.UserId == request.ManagerId, includes, true);
                if(managerExist == null)
                {
                    return new ServiceResponse<GetInforOfBusinessByManagerIdResponse>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(managerExist.RoleId != 1)
                {
                    return new ServiceResponse<GetInforOfBusinessByManagerIdResponse>
                    {
                        Message = "Tài khoản không phải là quản lý của doanh nghiệp.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var _mapper = config.CreateMapper();
                var entity = _mapper.Map<GetInforOfBusinessByManagerIdResponse>(managerExist);
                return new ServiceResponse<GetInforOfBusinessByManagerIdResponse>
                {
                    Data = entity,
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
