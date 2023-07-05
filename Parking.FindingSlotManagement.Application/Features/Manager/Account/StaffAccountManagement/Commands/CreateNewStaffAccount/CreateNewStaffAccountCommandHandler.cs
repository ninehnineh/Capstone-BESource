using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Account.StaffAccountManagement.Commands.CreateNewStaffAccount
{
    public class CreateNewStaffAccountCommandHandler : IRequestHandler<CreateNewStaffAccountCommand, ServiceResponse<int>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailService _emailService;
        MapperConfiguration _config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public CreateNewStaffAccountCommandHandler(IAccountRepository accountRepository, IEmailService emailService)
        {
            _accountRepository = accountRepository;
            _emailService = emailService;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewStaffAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExistByEmail = await _accountRepository.GetItemWithCondition(x => x.Email.Equals(request.Email), null, true);
                if (checkExistByEmail != null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Email đã tồn tại. Vui lòng nhập email khác!!!",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = _config.CreateMapper();
                var accEntity = _mapper.Map<User>(request);
                var passwordGenerate = GenerateRandomString(6);
                CreatePasswordHash(passwordGenerate, 
                    out byte[] passwordHash, 
                    out byte[] passwordSalt);
                accEntity.PasswordHash = passwordHash;
                accEntity.PasswordSalt = passwordSalt;
                accEntity.RoleId = 4;
                accEntity.IsActive = true;
                accEntity.IsCensorship = true;
                await _accountRepository.Insert(accEntity);
                EmailModel emailModel = new EmailModel();
                emailModel.To = accEntity.Email;
                emailModel.Subject = "Tài khoản nhân viên đã được quản lý thông qua.";
                emailModel.Body = "Chào bạn, Doanh nghiệp ParkZ của chúng tôi vô cùng hân hạnh khi được liên kết và làm việc với bạn. Dưới đây là thông tin đăng nhập vào trang web quản lý bãi xe dành cho nhân viên của bạn. Chúng tôi vô cùng hân hạnh được phục vụ bạn.";
                emailModel.Body += "\n Email: " + accEntity.Email;
                emailModel.Body += "\n Password: " + passwordGenerate;
                await _emailService.SendMail(emailModel);
                return new ServiceResponse<int>
                {
                    Data = accEntity.UserId,
                    Success = true,
                    StatusCode = 201,
                    Message = "Thành công"
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
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }
}
