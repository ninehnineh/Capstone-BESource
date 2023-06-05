using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.DisableOrEnableParkingPrice
{
    public class DisableOrEnableParkingPriceCommandHandler : IRequestHandler<DisableOrEnableParkingPriceCommand, ServiceResponse<string>>
    {
        private readonly IParkingPriceRepository _parkingPriceRepository;

        public DisableOrEnableParkingPriceCommandHandler(IParkingPriceRepository parkingPriceRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;
        }

        public async Task<ServiceResponse<string>> Handle(DisableOrEnableParkingPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingPrice = await _parkingPriceRepository
                    .GetItemWithCondition(x => x.ParkingPriceId == request.ParkingPriceId, null, false);

                parkingPrice.IsActive = !parkingPrice.IsActive;
                await _parkingPriceRepository.Save();

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
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
