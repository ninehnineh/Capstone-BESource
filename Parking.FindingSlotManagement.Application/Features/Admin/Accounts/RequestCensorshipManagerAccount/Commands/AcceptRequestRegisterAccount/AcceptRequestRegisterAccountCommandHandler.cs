using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Commands.AcceptRequestRegisterAccount
{
    public class AcceptRequestRegisterAccountCommandHandler : IRequestHandler<AcceptRequestRegisterAccountCommand, ServiceResponse<string>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailService _emailService;

        public AcceptRequestRegisterAccountCommandHandler(IAccountRepository accountRepository, IEmailService emailService)
        {
            _accountRepository = accountRepository;
            _emailService = emailService;
        }
        public async Task<ServiceResponse<string>> Handle(AcceptRequestRegisterAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _accountRepository.GetById(request.UserId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy tài khoản",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                checkExist.IsCensorship = true;
                await _accountRepository.Save();
                EmailModel emailModel = new EmailModel();
                emailModel.To = checkExist.Email;
                emailModel.Subject = "Tài khoản đã được doanh nghiêp ParkZ thông qua.";
                emailModel.Body = "Chào bạn, Doanh nghiệp ParkZ của chúng tôi vô cùng hân hạnh khi được liên kết và làm việc với bạn. Dưới đây là thông tin đăng nhập vào trang web quản lý bãi xe dành cho doanh nghiệp của bạn. Chúng tôi vô cùng hân hạnh được phục vụ bạn.";
                emailModel.Body += "\n Email: " + checkExist.Email;
                //emailModel.Body += "\n Password: " + checkExist.Password;
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
