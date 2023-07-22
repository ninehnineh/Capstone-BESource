using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Wallet.Queries.GetWalletByUserId
{
    public class GetWalletByUserIdQueryHandler : IRequestHandler<GetWalletByUserIdQuery, ServiceResponse<GetWalletByUserIdResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetWalletByUserIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetWalletByUserIdResponse>> Handle(GetWalletByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<User, object>>> includes = new()
                {
                    x => x.Wallet
                };
                var userExist = await _userRepository.GetItemWithCondition(x => x.UserId == request.UserId, includes);
                if (userExist == null)
                {
                    return new ServiceResponse<GetWalletByUserIdResponse>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var resEntity = _mapper.Map<GetWalletByUserIdResponse>(userExist.Wallet);
                return new ServiceResponse<GetWalletByUserIdResponse>
                {
                    Message = "Thành công",
                    Data = resEntity,
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
