using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.DeleteVnPay
{
    public class DeleteVnPayCommandHandler : IRequestHandler<DeleteVnPayCommand, ServiceResponse<string>>
    {
        private readonly IVnPayRepository _vnPayRepository;

        public DeleteVnPayCommandHandler(IVnPayRepository vnPayRepository)
        {
            _vnPayRepository = vnPayRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeleteVnPayCommand request, CancellationToken cancellationToken)
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
                await _vnPayRepository.Delete(checkExist);
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
