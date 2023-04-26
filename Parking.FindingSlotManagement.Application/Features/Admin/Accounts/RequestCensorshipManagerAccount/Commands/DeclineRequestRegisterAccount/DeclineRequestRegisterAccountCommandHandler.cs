using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Commands.DeclineRequestRegisterAccount
{
    public class DeclineRequestRegisterAccountCommandHandler : IRequestHandler<DeclineRequestRegisterAccountCommand, ServiceResponse<string>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailService _emailService;

        public DeclineRequestRegisterAccountCommandHandler(IAccountRepository accountRepository, IEmailService emailService)
        {
            _accountRepository = accountRepository;
            _emailService = emailService;
        }
        public async Task<ServiceResponse<string>> Handle(DeclineRequestRegisterAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _accountRepository.GetById(request.UserId);
                if (checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                checkExist.IsActive = false;
                checkExist.IsCensorship = false;
                await _accountRepository.Save();
                EmailModel emailModel = new EmailModel();
                emailModel.To = checkExist.Email;
                emailModel.Subject = "Tài khoản đã được doanh nghiêp ParkZ từ chối.";
                emailModel.Body = "Chào bạn, Doanh nghiệp ParkZ của chúng tôi vô cùng tiếc khi những thông tin mà bạn cung cấp không hợp lệ với các tiêu chí của doanh nghiệp. Hãy cung cấp những thông tin phù hợp với tiêu chí của chúng tôi để chúng ta có thể cùng hợp tác.";
                emailModel.Body += "Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.";
                await _emailService.SendMail(emailModel);
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 204,
                    Count = 0
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
