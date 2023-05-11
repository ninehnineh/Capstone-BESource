using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.UpdatePaypal
{
    public class UpdatePaypalCommandHandler : IRequestHandler<UpdatePaypalCommand, ServiceResponse<string>>
    {
        private readonly IPaypalRepository _paypalRepository;
        private readonly IAccountRepository _accountRepository;

        public UpdatePaypalCommandHandler(IPaypalRepository paypalRepository, IAccountRepository accountRepository)
        {
            _paypalRepository = paypalRepository;
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdatePaypalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _paypalRepository.GetById(request.PayPalId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy paypal.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(!string.IsNullOrEmpty(request.ClientId))
                {
                    checkExist.ClientId = request.ClientId;
                }
                if(!string.IsNullOrEmpty(request.SecretKey))
                {
                    checkExist.SecretKey = request.SecretKey;
                }
                if(!string.IsNullOrEmpty(request.ManagerId.ToString()))
                {
                    var checkManagerExist = await _accountRepository.GetById(request.ManagerId);
                    if (checkManagerExist == null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Không tìm thấy tài khoản.",
                            Success = true,
                            StatusCode = 200
                        };
                    }
                    if (checkManagerExist.IsActive != true || checkManagerExist.IsCensorship != true)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Tài khoản đang trong quá trình kiểm duyệt hoặc tài khoản bị ban.",
                            Success = true,
                            StatusCode = 200
                        };
                    }
                    if (checkManagerExist.RoleId != 1)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Tài khoản không phải là quản lý.",
                            Success = true,
                            StatusCode = 200
                        };
                    }
                    var checkManagerAlreadyHasRegisterPaypal = await _paypalRepository.GetItemWithCondition(x => x.ManagerId.Equals(checkManagerExist.UserId));
                    if (checkManagerAlreadyHasRegisterPaypal != null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Quản lý đã đăng ký tích hợp Paypal. Hãy chọn quản lý khác!!!",
                            Success = true,
                            StatusCode = 200,
                            Count = 0
                        };
                    }
                    checkExist.ManagerId = request.ManagerId;

                }
                await _paypalRepository.Update(checkExist);
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
    }
}
