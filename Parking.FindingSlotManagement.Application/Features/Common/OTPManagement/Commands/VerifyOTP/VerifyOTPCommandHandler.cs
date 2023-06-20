//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Common.OTPManagement.Commands.VerifyOTP
//{
//    public class VerifyOTPCommandHandler : IRequestHandler<VerifyOTPCommand, ServiceResponse<string>>
//    {
//        private readonly IOTPRepository _otpRepository;
//        private readonly IAccountRepository _accountRepository;
//        private readonly IEmailService _emailService;

//        public VerifyOTPCommandHandler(IOTPRepository otpRepository, IAccountRepository accountRepository, IEmailService emailService)
//        {
//            _otpRepository = otpRepository;
//            _accountRepository = accountRepository;
//            _emailService = emailService;
//        }
//        public async Task<ServiceResponse<string>> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
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
//                var otpExist = await _otpRepository.GetItemWithCondition(x => x.UserId == checkUserExist.UserId, null, false);
//                if(otpExist == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Không tìm thấy otp.",
//                        Success = true,
//                        StatusCode = 200
//                    };
//                }
//                if(otpExist.Code != request.OtpCode)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "OTP không hợp lệ.",
//                        StatusCode = 400,
//                        Success = false
//                    };
//                }
//                if(otpExist.ExpirationTime <= DateTime.UtcNow.AddHours(7))
//                {
//                    await _otpRepository.Delete(otpExist);
//                    return new ServiceResponse<string>
//                    {
//                        Message = "OTP đã hết hạn.",
//                        StatusCode = 400,
//                        Success = false
//                    };
//                }
//                await _otpRepository.Delete(otpExist);
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
//    }
//}
