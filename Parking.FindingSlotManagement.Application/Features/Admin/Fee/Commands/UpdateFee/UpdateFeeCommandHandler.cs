using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.UpdateFee
{
    public class UpdateFeeCommandHandler : IRequestHandler<UpdateFeeCommand, ServiceResponse<string>>
    {
        private readonly IFeeRepository _feeRepository;

        public UpdateFeeCommandHandler(IFeeRepository feeRepository)
        {
            _feeRepository = feeRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateFeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var feeExist = await _feeRepository.GetById(request.FeeId);
                if (feeExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(!string.IsNullOrEmpty(request.Name))
                {
                    feeExist.Name = request.Name;
                }
                if (!string.IsNullOrEmpty(request.BusinessType))
                {
                    feeExist.BusinessType = request.BusinessType;
                }
                if (request.Price > 0)
                {
                    feeExist.Price = request.Price;
                }
                await _feeRepository.Update(feeExist);
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
