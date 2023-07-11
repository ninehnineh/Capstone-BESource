using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.DeleteFee
{
    public class DeleteFeeCommandHandler : IRequestHandler<DeleteFeeCommand, ServiceResponse<string>>
    {
        private readonly IFeeRepository _feeRepository;

        public DeleteFeeCommandHandler(IFeeRepository feeRepository)
        {
            _feeRepository = feeRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeleteFeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var feeExist = await _feeRepository.GetById(request.FeeId);
                if(feeExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                await _feeRepository.Delete(feeExist);
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
