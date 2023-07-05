//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Models;
//using Parking.FindingSlotManagement.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Common.OTPManagement.Commands.GenerateOTP
//{
//    public class GenerateOTPCommandHandler : IRequestHandler<GenerateOTPCommand, ServiceResponse<string>>
//    {
//        //private readonly IOTPRepository _otpRepository;
//        private readonly IAccountRepository _accountRepository;
//        private readonly IEmailService _emailService;

//        public GenerateOTPCommandHandler(IOTPRepository otpRepository, IAccountRepository accountRepository, IEmailService emailService)
//        {
//            _otpRepository = otpRepository;
//            _accountRepository = accountRepository;
//            _emailService = emailService;
//        }
//        public async Task<ServiceResponse<string>> Handle(GenerateOTPCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var checkUserExist = await _accountRepository.GetItemWithCondition(x => x.Email.Equals(request.Email) && x.IsActive == true && x.IsCensorship == true, null, true);
//                if (checkUserExist == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Không tìm thấy tài khoản hoặc tài khoản đã bị ban.",
//                        StatusCode = 200,
//                        Success = true
//                    };
//                }
//                var checkOtpExistFromThisAccount = await _otpRepository.GetItemWithCondition(x => x.UserId == checkUserExist.UserId, null, false);
//                if(checkOtpExistFromThisAccount != null)
//                {
//                    await _otpRepository.Delete(checkOtpExistFromThisAccount);
//                }
//                var otpExpirationTime = GetOtpExpirationTime();
//                var otp = GenerateOtp();
//                var otpEntity = new OTP
//                {
//                    Code = otp,
//                    ExpirationTime = otpExpirationTime,
//                    UserId = checkUserExist.UserId
//                };
//                await _otpRepository.Insert(otpEntity);
//                EmailModel emailModel = new EmailModel();
//                emailModel.To = checkUserExist.Email;
//                emailModel.Subject = "Your OTP code";
//                emailModel.Body = $"Your OTP code is {otp}. It will expire in {otpExpirationTime} seconds.";
//                await _emailService.SendMail(emailModel);
//                return new ServiceResponse<string>
//                {
//                    Message = "Thành công",
//                    Success = true,
//                    StatusCode = 201
//                };
//            }
//            catch (Exception ex)
//            {

//                throw new Exception(ex.Message);
//            }
//        }
//        private string GenerateOtp()
//        {
//            const string chars = "0123456789";
//            var random = new Random();
//            var otp = new StringBuilder(6);

//            for (int i = 0; i < 6; i++)
//            {
//                otp.Append(chars[random.Next(chars.Length)]);
//            }

//            return otp.ToString();
//        }
//        private DateTime GetOtpExpirationTime()
//        {
//            return DateTime.UtcNow.AddHours(7).AddSeconds(60);
//        }
//    }
//}
