using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Wallet.Queries.GetWalletForAdmin
{
    public class GetWalletForAdminQueryHandler : IRequestHandler<GetWalletForAdminQuery, ServiceResponse<GetWalletForAdminResponse>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IMapper _mapper;

        public GetWalletForAdminQueryHandler(IWalletRepository walletRepository, IMapper mapper)
        {
            _walletRepository = walletRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetWalletForAdminResponse>> Handle(GetWalletForAdminQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = await _walletRepository.GetById(1);
                GetWalletForAdminResponse entityRes = _mapper.Map<GetWalletForAdminResponse>(wallet);
                return new ServiceResponse<GetWalletForAdminResponse>
                {
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
                    Data = entityRes
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
