using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.CreateNewPaypal
{
    public class CreateNewPaypalCommandHandler : IRequestHandler<CreateNewPaypalCommand, ServiceResponse<int>>
    {
        private readonly IPaypalRepository _paypalRepository;
        private readonly IAccountRepository _accountRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        public CreateNewPaypalCommandHandler(IPaypalRepository paypalRepository, IAccountRepository accountRepository)
        {
            _paypalRepository = paypalRepository;
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewPaypalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkManagerExist = await _accountRepository.GetById(request.ManagerId);
                if(checkManagerExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy tài khoản.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkManagerExist.IsActive != true || checkManagerExist.IsCensorship != true)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Tài khoản đang trong quá trình kiểm duyệt hoặc tài khoản bị ban.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkManagerExist.RoleId != 1)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Tài khoản không phải là quản lý.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                var _mapper = config.CreateMapper();
                var paypalEntity = _mapper.Map<PayPal>(request);
                await _paypalRepository.Insert(paypalEntity);
                return new ServiceResponse<int>
                {
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 201,
                    Data = paypalEntity.PayPalId
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
