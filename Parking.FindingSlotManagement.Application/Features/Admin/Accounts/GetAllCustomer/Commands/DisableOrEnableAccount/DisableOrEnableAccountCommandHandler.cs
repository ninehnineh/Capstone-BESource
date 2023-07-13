using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Commands.DisableOrEnableAccount
{
    public class DisableOrEnableAccountCommandHandler : IRequestHandler<DisableOrEnableAccountCommand, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;

        public DisableOrEnableAccountCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DisableOrEnableAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userExist = await _userRepository.GetById(request.UserId);
                if(userExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(userExist.IsActive == true)
                {
                    userExist.IsActive = false;
                }
                else if(userExist.IsActive == false)
                {
                    userExist.IsActive = true;
                }
                await _userRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
