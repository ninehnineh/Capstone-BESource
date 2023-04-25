using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.Commands.DeleteCensorshipManagerAccount
{
    public class DisableOrEnableCensorshipManagerAccountCommandHandler : IRequestHandler<DisableOrEnableManagerAccountCommand, ServiceResponse<string>>
    {
        private readonly IAccountRepository _accountRepository;

        public DisableOrEnableCensorshipManagerAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DisableOrEnableManagerAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var managerToDelete = await _accountRepository.GetById(request.UserId);
                if(managerToDelete == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Not found manager",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                if(managerToDelete.IsActive == true)
                {
                    managerToDelete.IsActive = false;
                }
                else if(managerToDelete.IsActive == false)
                {
                    managerToDelete.IsActive = true;
                }
                await _accountRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Successfully",
                    StatusCode = 204,
                    Success = true,
                    Count = 0
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
