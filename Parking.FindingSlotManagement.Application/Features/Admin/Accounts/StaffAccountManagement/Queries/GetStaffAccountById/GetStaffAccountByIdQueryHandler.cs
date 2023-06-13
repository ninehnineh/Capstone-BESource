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

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Queries.GetStaffAccountById
{
    public class GetStaffAccountByIdQueryHandler : IRequestHandler<GetStaffAccountByIdQuery, ServiceResponse<GetStaffAccountByIdResponse>>
    {
        private readonly IUserRepository _userRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public GetStaffAccountByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<GetStaffAccountByIdResponse>> Handle(GetStaffAccountByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<User, object>>> includes = new List<Expression<Func<User, object>>>
                {
                    x => x.Role
                };
                var staff = await _userRepository.GetItemWithCondition(x => x.UserId == request.UserId, includes, true);
                var _mapper = config.CreateMapper();
                var entity = _mapper.Map<GetStaffAccountByIdResponse>(staff);
                if(entity == null)
                {
                    return new ServiceResponse<GetStaffAccountByIdResponse>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(staff.RoleId != 4)
                {
                    return new ServiceResponse<GetStaffAccountByIdResponse>
                    {
                        Message = "Tài khoản không phải staff.",
                        Success = true,
                        StatusCode = 400
                    };
                }
                return new ServiceResponse<GetStaffAccountByIdResponse>
                {
                    Data = entity,
                    Success = true,
                    StatusCode = 200,
                    Message = "Thành công"
                };
             }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
