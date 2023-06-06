/*using FluentValidation;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.DisableOrEnableParkingPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.UpdateParkingPrice
{
    public class UpdateParkingPriceCommandValidation : AbstractValidator<UpdateParkingPriceCommand>
    {
        private readonly IParkingPriceRepository _parkingPriceRepository;

        public UpdateParkingPriceCommandValidation(IParkingPriceRepository parkingPriceRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;
            RuleFor(x => x.ParkingPriceId)
                .GreaterThan(0)
                .MustAsync(async (ParkingPriceId, token) =>
                {
                    var exist = await _parkingPriceRepository
                        .GetItemWithCondition(x => x.ParkingPriceId == ParkingPriceId);
                    return exist != null;
                }).WithMessage("'{PropertyName}' không tồn tại");


            RuleFor(p => p.ParkingPriceName)
                .MaximumLength(250).WithMessage("{PropertyName} không được nhập quá 250 kí tự")
                .MustAsync(async (command, context , token) =>
                {
                    var exists = await _parkingPriceRepository
                        .GetItemWithCondition(x => x.ParkingPriceName!.Equals(command.ParkingPriceName) &&
                                                    x.UserId == command.BusinessId);
                    return exists == null;
                }).WithMessage("'{PropertyValue}' đã tồn tại");

        }
    }
}
*/