using MediatR;
using NuGet.Protocol.Plugins;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.UpdateParkingPrice
{
    public class UpdateParkingPriceCommandHandler : IRequestHandler<UpdateParkingPriceCommand, ServiceResponse<string>>
    {
        private readonly IParkingPriceRepository _parkingPriceRepository;

        public UpdateParkingPriceCommandHandler(IParkingPriceRepository parkingPriceRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;
        }

        public async Task<ServiceResponse<string>> Handle(UpdateParkingPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingPrice = await _parkingPriceRepository
                    .GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, false);

                parkingPrice.ParkingPriceName = request.ParkingPriceName;
                await _parkingPriceRepository.Save();

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
