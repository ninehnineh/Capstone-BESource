using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.DisableOrEnablePackagePrice
{
    public class DisableOrEnablePackagePriceCommandHandler : IRequestHandler<DisableOrEnablePackagePriceCommand, ServiceResponse<string>>
    {
        private readonly IPackagePriceRepository _packagePriceRepository;

        public DisableOrEnablePackagePriceCommandHandler(IPackagePriceRepository packagePriceRepository)
        {
            _packagePriceRepository = packagePriceRepository;
        }
        public async Task<ServiceResponse<string>> Handle(DisableOrEnablePackagePriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _packagePriceRepository.GetById(request.PackagePriceId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Không tìm thấy gói.",
                        Success = true,
                        StatusCode = 200
                    };
                }
                if(checkExist.IsActive == true)
                {
                    checkExist.IsActive = false;
                }
                else if(checkExist.IsActive == false)
                {
                    checkExist.IsActive = true;
                }
                await _packagePriceRepository.Save();
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
