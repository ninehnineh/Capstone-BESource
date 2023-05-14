using FluentValidation;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.CreateParkingPrice
{
    public class CreateParkingPriceCommandValidation : AbstractValidator<CreateParkingPriceCommand>
    {
        private readonly IParkingPriceRepository _parkingPriceRepository;
        private readonly IUserRepository _userRepository;

        public CreateParkingPriceCommandValidation(IParkingPriceRepository parkingPriceRepository,
            IUserRepository userRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;
            _userRepository = userRepository;

            RuleFor(x => x.BusinessId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var exist = await _userRepository.GetItemWithCondition(x => x.UserId == id);
                    return exist != null;
                }).WithMessage("Business không tồn tại");

            RuleFor(p => p.ParkingPriceName)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
                .NotNull()
                .MaximumLength(250).WithMessage("{PropertyName} không được nhập quá 250 kí tự")
                .MustAsync(async (Command, context, token) =>
                {
                    var exists = await _parkingPriceRepository
                        .GetItemWithCondition(x => x.ParkingPriceName!.Equals(Command.ParkingPriceName) &&
                                                    x.UserId == Command.BusinessId);
                    return exists == null;
                }).WithMessage("'{PropertyValue}' đã tồn tại");
        }
    }
}
