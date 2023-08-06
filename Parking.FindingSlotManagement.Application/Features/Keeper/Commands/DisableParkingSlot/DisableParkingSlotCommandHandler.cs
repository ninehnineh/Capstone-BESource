using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.ParkingSlots.Commands.DisableParkingSlot;
using Parking.FindingSlotManagement.Application.Models.ParkingSlot;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.ParkingSlots.Commands.UpdateParkingSlotStatus
{
    public class DisableParkingSlotCommandHandler : IRequestHandler<DisableParkingSlotCommand, ServiceResponse<string>>
    {
        private readonly IParkingSlotRepository parkingSlotRepository;
        private readonly ITimeSlotRepository timeSlotRepository;
        private readonly IParkingRepository parkingRepository;
        private readonly IConflictRequestRepository conflictRequestRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IWalletRepository walletRepository;

        public DisableParkingSlotCommandHandler(IParkingSlotRepository parkingSlotRepository, ITimeSlotRepository timeSlotRepository,
            IParkingRepository parkingRepository, IConflictRequestRepository conflictRequestRepository,
            ITransactionRepository transactionRepository, IWalletRepository walletRepository)
        {
            this.conflictRequestRepository = conflictRequestRepository;
            this.transactionRepository = transactionRepository;
            this.walletRepository = walletRepository;
            this.parkingSlotRepository = parkingSlotRepository;
            this.timeSlotRepository = timeSlotRepository;
            this.parkingRepository = parkingRepository;
        }

        public async Task<ServiceResponse<string>> Handle(DisableParkingSlotCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var parkingSlotId = request.ParkingSlotId;
                ArgumentNullException.ThrowIfNull(parkingSlotId);

                var bookedTimeSlots = await timeSlotRepository.GetBookedTimeSlotIncludeBookingDetails(parkingSlotId);
                if (bookedTimeSlots == null)
                {
                    await parkingSlotRepository.DisableParkingSlotWhenAllTimeFree(parkingSlotId);
                    await timeSlotRepository.DisableTimeSlot(parkingSlotId);
                }
                else if (bookedTimeSlots != null)
                {
                    var parking = await parkingRepository.GetParking(parkingSlotId);
                    var tempListBookedTimeSlot = new List<DisableSlotResult>();

                    foreach (var item in bookedTimeSlots)
                    {
                        if (!tempListBookedTimeSlot.Any(x => x.BookingId == item.BookingId))
                        {
                            tempListBookedTimeSlot.Add(item);
                        }
                    }
                    
                    foreach (var result in tempListBookedTimeSlot)
                    {
                        var newConflictRequest = new ConflictRequest
                        {
                            BookingId = result.BookingId,
                            ParkingId = parking.ParkingId,
                            Status = "Process",
                            Message = request.Reason,
                        };
                        await conflictRequestRepository.Insert(newConflictRequest);
                    }
                    await parkingSlotRepository.DisableParkingSlotWhenAllTimeFree(parkingSlotId);
                    await timeSlotRepository.DisableTimeSlot(parkingSlotId);
                }

                return new ServiceResponse<string>
                {
                    Message = "Thành công",
                    StatusCode = 204,
                    Success = true
                };
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at DisableParkingSlotCommandHandler: {ex.Message}");
            }
        }
    }
}