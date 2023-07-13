using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.UpdateStaffAccount
{
    public class UpdateStaffAccountCommandHandler : IRequestHandler<UpdateStaffAccountCommand, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateStaffAccountCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateStaffAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _userRepository.GetById(request.UserId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(!string.IsNullOrEmpty(request.Name))
                {
                    checkExist.Name = request.Name;
                }
                if(!string.IsNullOrEmpty(request.Avatar))
                {
                    checkExist.Avatar = request.Avatar;
                }
                if(!string.IsNullOrEmpty(request.DateOfBirth.ToString()))
                {
                    checkExist.DateOfBirth = request.DateOfBirth;
                }
                if(!string.IsNullOrEmpty(request.Gender))
                {
                    checkExist.Gender = request.Gender;
                }

                await _userRepository.Update(checkExist);
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