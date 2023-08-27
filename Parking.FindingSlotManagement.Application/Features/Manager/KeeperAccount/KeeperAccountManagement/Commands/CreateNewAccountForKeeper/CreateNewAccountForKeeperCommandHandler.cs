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

namespace Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Commands.CreateNewAccountForKeeper
{
    public class CreateNewAccountForKeeperCommandHandler : IRequestHandler<CreateNewAccountForKeeperCommand, ServiceResponse<int>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IParkingRepository _parkingRepository;
        MapperConfiguration _config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewAccountForKeeperCommandHandler(IUserRepository userRepository, IEmailService emailService, IParkingRepository parkingRepository)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _parkingRepository = parkingRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewAccountForKeeperCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userExistBaseOnPhone = await _userRepository.GetItemWithCondition(x => x.Phone.Equals(request.Phone));
                if(userExistBaseOnPhone != null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Số điện thoại đã tồn tại. Vui lòng nhập số điện thoại khác!!!",
                        StatusCode = 400,
                        Success = true
                    };
                }
                var userExistBaseOnEmail = await _userRepository.GetItemWithCondition(x => x.Email.Equals(request.Email));
                if(userExistBaseOnEmail != null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Email đã tồn tại. Vui lòng nhập địa chỉ email khác!!!",
                        StatusCode = 400,
                        Success = true
                    };
                };
                var managerExist = await _userRepository.GetById(request.ManagerId);
                if(managerExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy tài khoản quản lý.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if(managerExist.RoleId != 1)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Tài khoản không phải là quản lý",
                        StatusCode = 400,
                        Success = true
                    };
                }

                var checkParkingExist = await _parkingRepository.GetById(request.ParkingId);
                if(checkParkingExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                if(checkParkingExist.IsActive == false)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Bãi đã bị ban nên không thể thêm nhân viên vào bãi.",
                        StatusCode = 400,
                        Success = false
                    };
                }

                var _mapper = _config.CreateMapper();
                var accEntity = _mapper.Map<User>(request);
                var passwordGenerate = "123@@";
                CreatePasswordHash(passwordGenerate,
                    out byte[] passwordHash,
                    out byte[] passwordSalt);
                accEntity.PasswordHash = passwordHash;
                accEntity.PasswordSalt = passwordSalt;
                accEntity.RoleId = 2;
                accEntity.IsActive = true;
                accEntity.IsCensorship = true;
                await _userRepository.Insert(accEntity);
                EmailModel emailModel = new EmailModel();
                emailModel.To = accEntity.Email;
                emailModel.Subject = "Tài khoản nhân viên đã được bãi giữ xe tạo.";
                emailModel.Body = "Chào bạn, Dưới đây là thông tin đăng nhập vào trang web quản lý bãi xe dành cho nhân viên của bạn. Chúng tôi vô cùng hân hạnh được phục vụ bạn.";
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
