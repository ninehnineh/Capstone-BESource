using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.CreateNewVnPay
{
    public class CreateNewVnPayCommandHandler : IRequestHandler<CreateNewVnPayCommand, ServiceResponse<int>>
    {
        private readonly IVnPayRepository _vnPayRepository;
        private readonly IAccountRepository _accountRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewVnPayCommandHandler(IVnPayRepository vnPayRepository, IAccountRepository accountRepository)
        {
            _vnPayRepository = vnPayRepository;
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewVnPayCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkManagerExist = await _accountRepository.GetItemWithCondition(x => x.RoleId == 1 && x.IsActive == true && x.IsCensorship == true && x.UserId.Equals(request.ManagerId));
                if (checkManagerExist == null)
                {
                    return new ServiceResponse<int>
                    {
                        Message = "Không tìm thấy quản lý hoặc quản lý không chưa được kiểm duyệt hoặc bị banned.",
                        Success = true,
                        StatusCode = 200,
                        Count = 0
                    };
                }
                var _mapper = config.CreateMapper();
                var vnPayEntity = _mapper.Map<Domain.Entities.VnPay>(request);
                await _vnPayRepository.Insert(vnPayEntity);
                return new ServiceResponse<int>
                {
                    Success = true,
                    StatusCode = 201,
                    Count = 0,
                    Data = vnPayEntity.VnPayId,
                    Message = "Thành công",
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
