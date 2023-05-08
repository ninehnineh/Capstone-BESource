using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.Accounts.StaffAccountManagement.Commands.UpdatePasswordByStaffId
{
    public class UpdatePasswordByStaffIdCommandHandler : IRequestHandler<UpdatePasswordByStaffIdCommand, ServiceResponse<string>>
    {
        private readonly IAccountRepository _accountRepository;

        public UpdatePasswordByStaffIdCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdatePasswordByStaffIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkManagerExist = await _accountRepository.GetById(request.UserId);
                if (checkManagerExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if (VerifyPasswordHash(request.OldPassword, checkManagerExist.PasswordHash, checkManagerExist.PasswordSalt) == false)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Mật khẩu cũ không đúng. Vui lòng nhập lại!!!",
                        Success = false,
                        StatusCode = 400
                    };
                }
                CreatePasswordHash(request.NewPassword,
                    out byte[] passwordHash,
                    out byte[] passwordSalt);
                checkManagerExist.PasswordHash = passwordHash;
                checkManagerExist.PasswordSalt = passwordSalt;
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
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
