using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ServiceResponse<GetCustomerByIdResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetCustomerByIdResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<User, object>>> includes = new()
                {
                    x => x.Role
                };
                var user = await _userRepository.GetItemWithCondition(x => x.UserId == request.UserId, includes);
                if(user == null)
                {
                    return new ServiceResponse<GetCustomerByIdResponse>
                    {
                        Message = "Không tìm thấy thông tin tài khoản.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var entity = _mapper.Map<GetCustomerByIdResponse>(user);
                return new ServiceResponse<GetCustomerByIdResponse>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200,
                    Data = entity
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
