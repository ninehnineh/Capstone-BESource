﻿using FluentValidation;
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
        private readonly ITrafficRepository _trafficRepository;

        public CreateParkingPriceCommandValidation(IParkingPriceRepository parkingPriceRepository,
            IUserRepository userRepository, ITrafficRepository trafficRepository)
        {
            _parkingPriceRepository = parkingPriceRepository;
            _userRepository = userRepository;
            _trafficRepository = trafficRepository;
            RuleFor(x => x.BusinessId)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
                .NotNull()
                .GreaterThanOrEqualTo(0).WithMessage("{BusinessId} phải lớn hơn 0")
                .MustAsync(async (id, token) =>
                {
                    var exist = await _userRepository.GetItemWithCondition(x => x.UserId == id);
                    return exist != null;
                }).WithMessage("Business không tồn tại");

            RuleFor(x => x.TrafficId)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
                .NotNull()
                .GreaterThanOrEqualTo(0).WithMessage("{TrafficId} phải lớn hơn 0")
                .MustAsync(async (id, token) =>
                {
                    var exist = await _trafficRepository.GetById(id);
                    return exist != null;
                }).WithMessage("Phương tiện không tồn tại");

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
