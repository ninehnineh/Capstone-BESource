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
// Keeper/Manager có thể disable một parking slot cụ thể nếu muốn trong trường hợp hư hỏng hoặc slot ko thể sử dùng
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
        private readonly IBookingRepository bookingRepository;

        public DisableParkingSlotCommandHandler(IParkingSlotRepository parkingSlotRepository, ITimeSlotRepository timeSlotRepository,
            IParkingRepository parkingRepository, IConflictRequestRepository conflictRequestRepository,
            ITransactionRepository transactionRepository, IWalletRepository walletRepository,
            IBookingRepository bookingRepository)
        {
            this.conflictRequestRepository = conflictRequestRepository;
            this.transactionRepository = transactionRepository;
            this.walletRepository = walletRepository;
            this.bookingRepository = bookingRepository;
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

                var parkingIsAvailable = await parkingSlotRepository.GetParkingByParkingSlotId(parkingSlotId);
                if (!parkingIsAvailable)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Bãi xe đang không hoạt động, không để bảo trì chỗ đỗ xe",
                        Success = true,
                        StatusCode = 200,
                    };
                }

                var slotIsInCheckIn = await bookingRepository.GetBookingStatusByParkingSlotId(parkingSlotId);
                if (slotIsInCheckIn)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Chỗ để xe đang được sử dụng, không thể bảo trì",
                        StatusCode = 200,
                    };
                }

                var bookedTimeSlots = await timeSlotRepository.GetBookedTimeSlotIncludeBookingDetails(parkingSlotId);
                if (bookedTimeSlots == null)
                {
                    // await parkingSlotRepository.DisableParkingSlotWhenAllTimeFree(parkingSlotId);
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
                            Status = ConflictRequestStatus.InProcess.ToString(),
                            Message = $"{ConflictRequestMessage.Bao_tri.ToString()} - {request.Reason}",
                        };
                        await conflictRequestRepository.Insert(newConflictRequest);
                        // List<Expression<Func<Domain.Entities.Transaction, object>>> includes = new()
                        // {
                        //     x => x.Wallet
                        // };
                        // var transaction = await transactionRepository.GetItemWithCondition(x => x.BookingId == result.BookingId, includes, false);
                        // var isPrePaid = transaction!.PaymentMethod!.Equals(PaymentMethod.tra_truoc.ToString());
                        // if (isPrePaid)
                        // {

                        //     var paidMoney = transaction.Price;
                        //     var userWalletId = transaction.WalletId;
                        //     var parkingManagerId = parking.ManagerId;

                        //     var userWallet = await walletRepository.GetItemWithCondition(x => x.WalletId == userWalletId, null, false);
                        //     var managerWallet = await walletRepository.GetItemWithCondition(x => x.UserId == parkingManagerId, null, false);
                        //     managerWallet.Balance -= paidMoney;
                        //     userWallet.Balance += paidMoney;
                        //     transaction.Status = "Huy";
                        //     transaction.Description = $"{request.Reason}";

                        //     await walletRepository.Save();
                        //     await transactionRepository.Save();
                        // }
                    }
                    // await parkingSlotRepository.DisableParkingSlotWhenAllTimeFree(parkingSlotId); 
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
                throw new Exception($"Error at DisableParkingSlotCommandHandler: Message {ex.Message}");
            }
        }
    }
}