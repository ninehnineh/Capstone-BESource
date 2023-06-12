using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.DisableOrEnableStaffAccount
{
    public class DisableOrEnableStaffAccountCommandHandler : IRequestHandler<DisableOrEnableStaffAccountCommand, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;

        public DisableOrEnableStaffAccountCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DisableOrEnableStaffAccountCommand request, CancellationToken cancellationToken)
        {
            var staffToDelete = await _userRepository.GetById(request.UserId);
            if (staffToDelete == null)
            {
                return new ServiceResponse<string>
                {
                    Message = "Không tìm thấy tài khoản",
                    Success = true,
                    StatusCode = 200,
                    Count = 0
                };
            }
            if (staffToDelete.IsActive == true)
            {
                staffToDelete.IsActive = false;
            }
            else if (staffToDelete.IsActive == false)
            {
                staffToDelete.IsActive = true;
            }
            await _userRepository.Save();
            return new ServiceResponse<string>
            {
                Message = "Thành công",
                StatusCode = 204,
                Success = true,
                Count = 0
            };
        }
    }
}
