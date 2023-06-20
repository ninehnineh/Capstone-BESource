using FluentValidation;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.Create
{
    public class CreateParkingSlotsCommandValidation : AbstractValidator<CreateParkingSlotsCommand>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly ITrafficRepository _trafficRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IBookingRepository _bookingRepository;

        public CreateParkingSlotsCommandValidation(IParkingRepository parkingRepository, 
            ITrafficRepository trafficRepository,
            IFloorRepository floorRepository,
            IBookingRepository bookingRepository)
        {

            _parkingRepository = parkingRepository;
            _trafficRepository = trafficRepository;
            _floorRepository = floorRepository;
            _bookingRepository = bookingRepository;

            RuleFor(x => x.Name)
                .NotNull().WithMessage("{PropertyName} không nhận giá trị null")
                .NotEmpty().WithMessage("{PropertyName} không được để trống")
                .MaximumLength(10).WithMessage("{PropertyName} không được vượt quá {MaxLength}");

            RuleFor(x => x.RowIndex)
                .NotNull().WithMessage("Vui lòng nhập {PropertyName}")
                .NotEmpty().WithMessage("{PropertyName} không được để trống");
            
            RuleFor(x => x.RowIndex)
                .NotNull().WithMessage("Vui lòng nhập {PropertyName}")
                .NotEmpty().WithMessage("{PropertyName} không được để trống");

            RuleFor(x => x.IsAvailable)
                .NotNull().WithMessage("Vui lòng nhập {PropertyName}")
                .NotEmpty().WithMessage("{PropertyName} không được để trống");

            RuleFor(x => x.ParkingId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var exists = await _parkingRepository.GetById(id!);
                    return exists != null;
                }).WithMessage("{PropertyName} không tồn tại");

            RuleFor(x => x.TrafficId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var exists = await _trafficRepository.GetById(id!);
                    return exists != null;
                }).WithMessage("{PropertyName} không tồn tại");

            RuleFor(x => x.FloorId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var exists = await _floorRepository.GetById(id!);
                    return exists != null;
                }).WithMessage("{PropertyName} không tồn tại");

        }
    }
}
