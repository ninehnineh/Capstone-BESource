using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Commands.UpdatePasswordForStaff
{
    public class UpdatePasswordForStaffCommandHandler : IRequestHandler<UpdatePasswordForStaffCommand, ServiceResponse<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UpdatePasswordForStaffCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }
        public async Task<ServiceResponse<string>> Handle(UpdatePasswordForStaffCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var staffExist = await _userRepository.GetById(request.UserId);
                if(staffExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if (staffExist.RoleId != 4)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Tài khoản không phải staff nên không thể cấp lại mật khẩu.",
                        Success = true,
                        StatusCode = 400
                    };
                }
                CreatePasswordHash(request.NewPassword,
                out byte[] passwordHash,
                out byte[] passwordSalt);
                staffExist.PasswordHash = passwordHash;
                staffExist.PasswordSalt = passwordSalt;
                await _userRepository.Save();
                EmailModel emailModel = new EmailModel();
                emailModel.To = staffExist.Email;
                emailModel.Subject = "Tài khoản đã được doanh nghiêp ParkZ cấp lại mật khẩu.";
                emailModel.Body = "Chào bạn, Doanh nghiệp ParkZ của chúng tôi vô cùng hân hạnh khi được liên kết và làm việc với bạn. Dưới đây là thông tin đăng nhập vào trang web quản lý bãi xe dành cho doanh nghiệp của bạn. Chúng tôi vô cùng hân hạnh được phục vụ bạn.";
                emailModel.Body += "\n Email: " + staffExist.Email;
                emailModel.Body += "\n Password: " + request.NewPassword;
                await _emailService.SendMail(emailModel);
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
