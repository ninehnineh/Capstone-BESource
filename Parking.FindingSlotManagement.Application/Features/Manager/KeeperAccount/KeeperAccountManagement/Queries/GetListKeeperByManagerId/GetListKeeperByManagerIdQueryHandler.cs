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

namespace Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetListKeeperByManagerId
{
    public class GetListKeeperByManagerIdQueryHandler : IRequestHandler<GetListKeeperByManagerIdQuery, ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>>
    {
        private readonly IUserRepository _userRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetListKeeperByManagerIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>> Handle(GetListKeeperByManagerIdQuery request, CancellationToken cancellationToken)
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
                var checkManager = await _userRepository.GetById(request.ManagerId);
                if (checkManager == null)
                {
                    return new ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>
                    {
                        Message = "Không tìm thấy tài khoản quản lý.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if (checkManager.RoleId != 1)
                {
                    return new ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>
                    {
                        Message = "Tài khoản không phải là quản lý nên không được quyền truy cập.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                List<Expression<Func<User, object>>> includes = new List<Expression<Func<User, object>>>
                {
                    x => x.Parking
                };
                var lstStaff = await _userRepository.GetAllItemWithPagination(x => x.ManagerId == request.ManagerId && x.IsActive == true, includes, x => x.UserId, true, request.PageNo, request.PageSize);
                if (!lstStaff.Any())
                {
                    return new ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>
                    {
                        Message = "Không tìm thấy.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                var _mapper = config.CreateMapper();
                var lstDto = _mapper.Map<IEnumerable<GetListKeeperByManagerIdResponse>>(lstStaff);
                return new ServiceResponse<IEnumerable<GetListKeeperByManagerIdResponse>>
                {
                    Data = lstDto,
                    Message = "Thành công",
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
