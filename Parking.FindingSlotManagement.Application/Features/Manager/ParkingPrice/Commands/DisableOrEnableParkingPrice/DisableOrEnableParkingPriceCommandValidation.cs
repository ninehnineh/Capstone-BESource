using FluentValidation;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.DisableOrEnableParkingPrice
{
    public class DisableOrEnableParkingPriceCommandValidation : AbstractValidator<DisableOrEnableParkingPriceCommand>
    {
        private readonly IParkingPriceRepository _parkingPriceRepository;

        public DisableOrEnableParkingPriceCommandValidation(IParkingPriceRepository parkingPriceRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;

            RuleFor(x => x.ParkingPriceId)
                .GreaterThan(0)
                .MustAsync(async (ParkingPriceId, token) =>
                {
                    var exist = await _parkingPriceRepository
                        .GetItemWithCondition(x => x.ParkingPriceId == ParkingPriceId);
                    return exist != null;
                }).WithMessage("'{PropertyValue}' không tồn tại");
        }
    }
}
