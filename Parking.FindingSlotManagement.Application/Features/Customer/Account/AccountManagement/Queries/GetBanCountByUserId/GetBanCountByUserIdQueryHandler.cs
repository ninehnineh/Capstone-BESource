using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Queries.GetBanCountByUserId
{
    public class GetBanCountByUserIdQueryHandler : IRequestHandler<GetBanCountByUserIdQuery, ServiceResponse<GetBanCountByUserIdResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetBanCountByUserIdQueryHandler(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetBanCountByUserIdResponse>> Handle(GetBanCountByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userExist = await _userRepository.GetById(request.UserId);
                if(userExist == null)
                {
                    return new ServiceResponse<GetBanCountByUserIdResponse>
                    {
                        Message = "Không tìm thấy người dùng.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var entityReturn = _mapper.Map<GetBanCountByUserIdResponse>(userExist);
                return new ServiceResponse<GetBanCountByUserIdResponse>
                {
                    Data = entityReturn,
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
