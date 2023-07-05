using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.OTPManagement.Commands.SendMailWithOTP
{
    public class SendMailWithOTPCommandsHandler : IRequestHandler<SendMailWithOTPCommands, ServiceResponse<string>>
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public SendMailWithOTPCommandsHandler(IEmailService emailService, IUserRepository userRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
        }
        public async Task<ServiceResponse<string>> Handle(SendMailWithOTPCommands request, CancellationToken cancellationToken)
        {
            try
            {
                var checkUserExist = await _userRepository.GetItemWithCondition(x => x.Email.Equals(request.Email) && x.IsActive == true && x.IsCensorship == true, null, true);
                if (checkUserExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản hoặc tài khoản đã bị ban.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                EmailModel emailModel = new EmailModel();
                emailModel.To = checkUserExist.Email;
                emailModel.Subject = "Your OTP code";
                emailModel.Body = $"Your OTP code is {request.OTP}.";
                await _emailService.SendMail(emailModel);
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
