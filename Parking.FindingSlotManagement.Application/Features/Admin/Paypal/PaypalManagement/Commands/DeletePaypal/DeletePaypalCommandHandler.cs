using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.DeletePaypal
{
    public class DeletePaypalCommandHandler : IRequestHandler<DeletePaypalCommand, ServiceResponse<string>>
    {
        private readonly IPaypalRepository _paypalRepository;

        public DeletePaypalCommandHandler(IPaypalRepository paypalRepository)
        {
            _paypalRepository = paypalRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DeletePaypalCommand request, CancellationToken cancellationToken)
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
                await _paypalRepository.Delete(checkExist);
                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
                    Success = true
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
