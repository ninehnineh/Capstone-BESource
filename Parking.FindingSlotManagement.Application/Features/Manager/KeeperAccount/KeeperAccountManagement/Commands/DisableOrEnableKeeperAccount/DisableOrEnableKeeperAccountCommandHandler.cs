using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Commands.DisableOrEnableKeeperAccount
{
    public class DisableOrEnableKeeperAccountCommandHandler : IRequestHandler<DisableOrEnableKeeperAccountCommand, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;

        public DisableOrEnableKeeperAccountCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DisableOrEnableKeeperAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _userRepository.GetById(request.UserId);
                if (checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if (checkExist.IsActive == false)
                {
                    checkExist.IsActive = true;
                }
                else if (checkExist.IsActive == true)
                {
                    checkExist.IsActive = false;
                }
                await _userRepository.Save();
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
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
