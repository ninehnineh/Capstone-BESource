using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.PasswordManagement.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ServiceResponse<string>>
    {
        private readonly IAccountRepository _accountRepository;

        public ForgotPasswordCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var acc = await _accountRepository.GetItemWithCondition(x => x.Email.Equals(request.Email), null, true);
                if(acc == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200,
                    };
                }
                if(acc.IsActive == false && acc.IsCensorship == false || acc.IsActive == true && acc.IsCensorship == false)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Tài khoản của bạn đang trong quá trình kiểm duyệt hoặc bị hệ thống Ban.",
                        Success = false,
                        StatusCode = 400
                    };
                }
                var accToEdit = await _accountRepository.GetById(acc.UserId);
                CreatePasswordHash(request.NewPassword, 
                    out byte[] passwordHash, 
                    out byte[] passwordSalt);
                accToEdit.PasswordHash = passwordHash;
                accToEdit.PasswordSalt = passwordSalt;
                await _accountRepository.Save();
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
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
