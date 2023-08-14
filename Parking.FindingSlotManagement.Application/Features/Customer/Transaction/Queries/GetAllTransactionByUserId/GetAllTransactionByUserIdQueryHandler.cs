using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Wallet.Queries.GetWalletByUserId;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Transaction.Queries.GetAllTransactionByUserId
{
    public class GetAllTransactionByUserIdQueryHandler : IRequestHandler<GetAllTransactionByUserIdQuery, ServiceResponse<IEnumerable<GetAllTransactionByUserIdResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllTransactionByUserIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<GetAllTransactionByUserIdResponse>>> Handle(GetAllTransactionByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<User, object>>> includes = new()
                {
                    x => x.Wallet,
                    x => x.Wallet.Transactions
                };
                var userExist = await _userRepository.GetItemWithCondition(x => x.UserId == request.UserId, includes);
                if (userExist == null)
                {
                    return new ServiceResponse<IEnumerable<GetAllTransactionByUserIdResponse>>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = false,
                        StatusCode = 404
                    };
                }
                var entityRes = _mapper.Map<IEnumerable<GetAllTransactionByUserIdResponse>>(userExist.Wallet.Transactions.OrderByDescending(x => x.TransactionId));
                return new ServiceResponse<IEnumerable<GetAllTransactionByUserIdResponse>>
                {
                    Message = "Thành công",
                    Data = entityRes,
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
