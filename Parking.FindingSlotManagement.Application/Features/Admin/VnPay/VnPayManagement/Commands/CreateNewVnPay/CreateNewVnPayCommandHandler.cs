using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.CreateNewVnPay
{
    public class CreateNewVnPayCommandHandler : IRequestHandler<CreateNewVnPayCommand, ServiceResponse<int>>
    {
        private readonly IVnPayRepository _vnPayRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IBusinessProfileRepository _businessProfileRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewVnPayCommandHandler(IVnPayRepository vnPayRepository, IAccountRepository accountRepository, IBusinessProfileRepository businessProfileRepository)
        {
            _vnPayRepository = vnPayRepository;
            _accountRepository = accountRepository;
            _businessProfileRepository = businessProfileRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewVnPayCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var accountExist = await _accountRepository.GetById(request.UserId);
                if(accountExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var vnPayEntity = _mapper.Map<Domain.Entities.VnPay>(request);
                await _vnPayRepository.Insert(vnPayEntity);
                return new ServiceResponse<int>
                {
                    Success = true,
                    StatusCode = 201,
                    Count = 0,
                    Data = vnPayEntity.VnPayId,
                    Message = "Thành công",
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
