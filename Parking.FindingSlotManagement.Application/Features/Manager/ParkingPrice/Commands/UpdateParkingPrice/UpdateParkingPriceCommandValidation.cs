using FluentValidation;
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
        private readonly IUserRepository _userRepository;

        public UpdateParkingPriceCommandValidation(IParkingPriceRepository parkingPriceRepository,
            IUserRepository userRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;
            _userRepository = userRepository;

            RuleFor(x => x.ParkingPriceId)
                .GreaterThan(0)
                .MustAsync(async (ParkingPriceId, token) =>
                {
                    var exist = await _parkingPriceRepository
                        .GetItemWithCondition(x => x.ParkingPriceId == ParkingPriceId);
                    return exist != null;
                }).WithMessage("'{PropertyValue}' không tồn tại");

            RuleFor(x => x.BusinessId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var exist = await _userRepository.GetItemWithCondition(x => x.UserId == id);
                    return exist != null;
                }).WithMessage("'{PropertyName}' không tồn tại");

            RuleFor(p => p.ParkingPriceName)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
                .NotNull()
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
