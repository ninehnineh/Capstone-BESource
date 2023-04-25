using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.CreateNewCensorshipManagerAccount
{
    public class CreateNewCensorshipManagerAccountCommandHandler : IRequestHandler<CreateNewCensorshipManagerAccountCommand, ServiceResponse<int>>
    {
        private readonly IAccountRepository _accountRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewCensorshipManagerAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewCensorshipManagerAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _accountRepository.GetItemWithCondition(x => x.Email.Equals(request.Email));
                if (checkExist != null)
                {
                    return new ServiceResponse<int>
                    {
                        StatusCode = 200,
                        Success = true,
                        Count = 0,
                        Message = "Email đã tồn tại. Vui lòng nhập lại email!!!"
                    };
                }
                var _mapper = config.CreateMapper();
                var managerEntity = _mapper.Map<User>(request);
                await _accountRepository.Insert(managerEntity);
                return new ServiceResponse<int>
                {
                    Data = managerEntity.UserId,
                    StatusCode = 201,
                    Success = true,
                    Count = 0,
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
