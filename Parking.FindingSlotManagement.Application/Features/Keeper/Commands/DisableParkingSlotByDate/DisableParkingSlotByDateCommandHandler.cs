using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.ParkingSlots.Commands.DisableParkingSlotByDate;
using Parking.FindingSlotManagement.Domain.Enum;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.DisableParkingSlotByDate
{

    public class DisableParkingSlotByDateCommandHandler : IRequestHandler<DisableParkingSlotByDateCommand, ServiceResponse<string>>
    {
        private readonly IParkingSlotRepository parkingSlotRepository;
        private readonly IParkingRepository parkingRepository;
        private readonly IMapper mapper;

        public DisableParkingSlotByDateCommandHandler(IParkingSlotRepository parkingSlotRepository, IParkingRepository parkingRepository,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.parkingRepository = parkingRepository;
            this.parkingSlotRepository = parkingSlotRepository;
        }

        public async Task<ServiceResponse<string>> Handle(DisableParkingSlotByDateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var disableDate = request.DisableDate;
                var parkingId = request.ParkingId;
                var reason = request.Reason;

                ArgumentNullException.ThrowIfNull(parkingId);
                ArgumentNullException.ThrowIfNull(disableDate);

                // if (DateTime.UtcNow.AddDays(2).Date == disableDate.Date)
                // {
                    
                // }

                var parkingIncludeTimeSlots = await parkingRepository.GetParkingById(parkingId);
                var floors = parkingIncludeTimeSlots!.Floors!;

                foreach (var floor in floors)
                {
                    var parkingSlots = floor!.ParkingSlots!;
                    foreach (var parkingSlot in parkingSlots)
                    {
                        var timeSlots = parkingSlot.TimeSlots;
                        foreach (var timeSlot in timeSlots)
                        {
                            if (timeSlot.StartTime.Date == disableDate.Date)
                            {
                                var isFree = timeSlot.Status.Equals(TimeSlotStatus.Free.ToString());
                                timeSlot.Status = isFree ? TimeSlotStatus.Busy.ToString() : TimeSlotStatus.Free.ToString();
                            }
                        }
                    }
                }

                await parkingSlotRepository.Save();

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 200,
                    Success = true
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at DisableParkingSlotByDateCommandHandler: Message {ex.Message}");
            }
        }
    }
}