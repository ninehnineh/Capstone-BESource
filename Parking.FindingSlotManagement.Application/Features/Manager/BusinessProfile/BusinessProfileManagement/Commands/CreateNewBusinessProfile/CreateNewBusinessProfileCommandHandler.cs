using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Commands.CreateNewBusinessProfile
{
    public class CreateNewBusinessProfileCommandHandler : IRequestHandler<CreateNewBusinessProfileCommand, ServiceResponse<int>>
    {
        private readonly IBusinessProfileRepository _businessProfileRepository;
        private readonly IAccountRepository _accountRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewBusinessProfileCommandHandler(IBusinessProfileRepository businessProfileRepository, IAccountRepository accountRepository)
        {
            _businessProfileRepository = businessProfileRepository;
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewBusinessProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _accountRepository.GetById(request.UserId);
                if(checkUserExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var businessProfileEntity = _mapper.Map<Domain.Entities.BusinessProfile>(request);
                await _businessProfileRepository.Insert(businessProfileEntity);
                return new ServiceResponse<int>
                {
                    Data = businessProfileEntity.BusinessProfileId,
                    Message = "Thành công",
                    StatusCode = 201,
                    Success = true
                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
