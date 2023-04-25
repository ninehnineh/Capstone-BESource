using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Commands.DeclineRequestRegisterAccount
{
    public class DeclineRequestRegisterAccountCommandHandler : IRequestHandler<DeclineRequestRegisterAccountCommand, ServiceResponse<string>>
    {
        private readonly IAccountRepository _accountRepository;

        public DeclineRequestRegisterAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeclineRequestRegisterAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _accountRepository.GetById(request.UserId);
                if (checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                checkExist.IsActive = false;
                checkExist.IsCensorship = false;
                await _accountRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204,
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
