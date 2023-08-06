/*using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.ParkingSlots.Commands.DisableParkingSlot;
using Parking.FindingSlotManagement.Application.Features.Keeper.ParkingSlots.Commands.UpdateParkingSlotStatus;
using Parking.FindingSlotManagement.Application.Models.ParkingSlot;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Keeper
{
    public class DisableParkingSlotCommandHandlerTests
    {
        private readonly Mock<IParkingSlotRepository> _parkingSlotRepositoryMock;
        private readonly Mock<ITimeSlotRepository> _timeSlotRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IConflictRequestRepository> _conflictRequestRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly DisableParkingSlotCommandHandler _handler;
        public DisableParkingSlotCommandHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _parkingSlotRepositoryMock = new Mock<IParkingSlotRepository>();
            _timeSlotRepositoryMock = new Mock<ITimeSlotRepository>();
            _conflictRequestRepositoryMock = new Mock<IConflictRequestRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _handler = new DisableParkingSlotCommandHandler(_parkingSlotRepositoryMock.Object, _timeSlotRepositoryMock.Object, _parkingRepositoryMock.Object, _conflictRequestRepositoryMock.Object, _transactionRepositoryMock.Object, _walletRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ExistingBookedTimeSlots_ShouldDisableParkingSlotAndCreateConflictRequests()
        {
            // Arrange
            int parkingSlotId = 1;
            var request = new DisableParkingSlotCommand { ParkingSlotId = parkingSlotId, Reason = "Reason for disabling" };

            // Mock the bookedTimeSlots to simulate booked time slots
            var bookedTimeSlots = new List<DisableSlotResult>
        {
            new DisableSlotResult { BookingId = 1 },
            new DisableSlotResult { BookingId = 2 },
            new DisableSlotResult { BookingId = 3 }
            // Add more booked time slots as needed...
        };



            // Setup mock repositories to return the expected data
            _timeSlotRepositoryMock.Setup(repo => repo.GetBookedTimeSlotIncludeBookingDetails(parkingSlotId))
                .ReturnsAsync(bookedTimeSlots);



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");
        }
    }
}
*/