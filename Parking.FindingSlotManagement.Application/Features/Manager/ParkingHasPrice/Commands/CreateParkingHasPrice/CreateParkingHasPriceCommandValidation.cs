using FluentValidation;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.CreateParkingHasPrice
{
    public class CreateParkingHasPriceCommandValidation : AbstractValidator<CreateParkingHasPriceCommand>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IPackagePriceRepository _packagePriceRepository;

        public CreateParkingHasPriceCommandValidation(IParkingRepository parkingRepository,
            IPackagePriceRepository packagePriceRepository)
        {
            _parkingRepository = parkingRepository;
            _packagePriceRepository = packagePriceRepository;

            RuleFor(x => x.ParkingId)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}")
                .NotNull()
                .GreaterThan(0)
                .MustAsync(async (ParkingId, token) =>
                    {
                        var exist = await _parkingRepository.GetById(ParkingId!);
                        return exist != null;
                    }).WithMessage("{PropertyName} không tồn tại");

            RuleFor(x => x.ParkingPriceId)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}")
                .NotNull()
                .GreaterThan(0)
                .MustAsync(async (ParkingPriceId, token) =>
                    {
                        var exist = await _packagePriceRepository.GetById(ParkingPriceId!);
                        return exist == null;
                    }).WithMessage("{PropertyName} không tồn tại");

        }
    }
}
