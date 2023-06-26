using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.VerifyEmail.Queries.VerifyEmailExist
{
    public class VerifyEmailExistQueryHandler : IRequestHandler<VerifyEmailExistQuery, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;

        public VerifyEmailExistQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<string>> Handle(VerifyEmailExistQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var checkEmailExist = await _userRepository.GetItemWithCondition(x => x.Email.Equals(request.Email.ToString()));
                if(checkEmailExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkEmailExist.RoleId != 1 && checkEmailExist.RoleId != 2)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Email không thuộc quyền truy cập hệ thống.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 200,
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
