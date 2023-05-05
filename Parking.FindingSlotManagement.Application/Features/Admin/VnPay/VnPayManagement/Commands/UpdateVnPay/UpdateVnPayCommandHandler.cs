using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.UpdateVnPay
{
    public class UpdateVnPayCommandHandler : IRequestHandler<UpdateVnPayCommand, ServiceResponse<string>>
    {
        private readonly IVnPayRepository _vnPayRepository;
        private readonly IAccountRepository _accountRepository;

        public UpdateVnPayCommandHandler(IVnPayRepository vnPayRepository, IAccountRepository accountRepository)
        {
            _vnPayRepository = vnPayRepository;
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateVnPayCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _vnPayRepository.GetById(request.VnPayId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                if(!string.IsNullOrEmpty(request.TmnCode))
                {
                    checkExist.TmnCode = request.TmnCode;
                }
                if(!string.IsNullOrEmpty(request.HashSecret))
                {
                    checkExist.HashSecret = request.HashSecret;
                }
                if(!string.IsNullOrEmpty(request.ManagerId.ToString()))
                {
                    var checkManagerExist = await _accountRepository.GetItemWithCondition(x => x.RoleId == 1 && x.IsActive == true && x.IsCensorship == true && x.UserId.Equals(request.ManagerId));
                    if(checkManagerExist == null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Không tìm thấy quản lý hoặc quản lý không chưa được kiểm duyệt hoặc bị banned.",
                            Success = true,
                            StatusCode = 200,
                            Count = 0
                        };
                    }
                    var checkManagerAlreadyHasRegisterVnPay = await _vnPayRepository.GetItemWithCondition(x => x.ManagerId.Equals(checkManagerExist.UserId));
                    if(checkManagerAlreadyHasRegisterVnPay != null)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Quản lý đã đăng ký tích hợp VnPay. Hãy chọn quản lý khác!!!",
                            Success = true,
                            StatusCode = 200,
                            Count = 0
                        };
                    }
                    checkExist.ManagerId = request.ManagerId;
                }
                await _vnPayRepository.Update(checkExist);
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
