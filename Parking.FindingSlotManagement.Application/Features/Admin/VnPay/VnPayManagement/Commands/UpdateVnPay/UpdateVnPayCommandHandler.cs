//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.UpdateVnPay
//{
//    public class UpdateVnPayCommandHandler : IRequestHandler<UpdateVnPayCommand, ServiceResponse<string>>
//    {
//        private readonly IVnPayRepository _vnPayRepository;
//        private readonly IBusinessProfileRepository _businessProfileRepository;
//        private readonly IAccountRepository _accountRepository;

//        public UpdateVnPayCommandHandler(IVnPayRepository vnPayRepository, IBusinessProfileRepository businessProfileRepository)
//        {
//            _vnPayRepository = vnPayRepository;
//            _businessProfileRepository = businessProfileRepository;
//        }
//        public async Task<ServiceResponse<string>> Handle(UpdateVnPayCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var checkExist = await _vnPayRepository.GetById(request.VnPayId);
//                if (checkExist == null)
//                {
//                    return new ServiceResponse<string>
//                    {
//                        Message = "Không tìm thấy.",
//                        Success = true,
//                        StatusCode = 200,
//                        Count = 0
//                    };
//                }
//                if (!string.IsNullOrEmpty(request.TmnCode))
//                {
//                    checkExist.TmnCode = request.TmnCode;
//                }
//                if (!string.IsNullOrEmpty(request.HashSecret))
//                {
//                    checkExist.HashSecret = request.HashSecret;
//                }
//                if (!string.IsNullOrEmpty(request.BusinessId.ToString()))
//                {
                    
//                    var checkBusinessExist = await _businessProfileRepository.GetById(request.BusinessId);
//                    if (checkBusinessExist == null)
//                    {
//                        return new ServiceResponse<string>
//                        {
//                            Message = "Không tìm thấy tài khoản doanh nghiêp.",
//                            Success = true,
//                            StatusCode = 200,
//                        };
//                    }
//                    var checkBusinessAccountAlreadyRegisterVnPay = await _vnPayRepository
//                        .GetItemWithCondition(x => x.BusinessId.Equals(checkBusinessExist.BusinessProfileId));
//                    if (checkBusinessAccountAlreadyRegisterVnPay != null)
//                    {
//                        return new ServiceResponse<string>
//                        {
//                            Message = "Quản lý đã đăng ký tích hợp VnPay. Hãy chọn quản lý khác!!!",
//                            Success = true,
//                            StatusCode = 200,
//                            Count = 0
//                        };
//                    }
//                    checkExist.BusinessId = request.BusinessId;
//                }
//                await _vnPayRepository.Update(checkExist);
//                return new ServiceResponse<string>
//                {
//                    Message = "Thành công",
//                    Success = true,
//                    StatusCode = 204
//                };
//            }
//            catch (Exception ex)
//            {

//                throw new Exception(ex.Message);
//            }
//        }
//    }
//}
