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

namespace Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetKeeperById
{
    public class GetKeeperByIdQueryHandler : IRequestHandler<GetKeeperByIdQuery, ServiceResponse<GetKeeperByIdResponse>>
    {
        private readonly IUserRepository _userRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public GetKeeperByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<GetKeeperByIdResponse>> Handle(GetKeeperByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<User, object>>> includes = new List<Expression<Func<User, object>>>
                {
                    x => x.Role,
                    x => x.Parking
                };
                var user = await _userRepository.GetItemWithCondition(x => x.UserId == request.UserId, includes, true);
                if(user == null)
                {
                    return new ServiceResponse<GetKeeperByIdResponse>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var _mapper = config.CreateMapper();
                var entityDto = _mapper.Map<GetKeeperByIdResponse>(user);
                return new ServiceResponse<GetKeeperByIdResponse>
                {
                    Data = entityDto,
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
