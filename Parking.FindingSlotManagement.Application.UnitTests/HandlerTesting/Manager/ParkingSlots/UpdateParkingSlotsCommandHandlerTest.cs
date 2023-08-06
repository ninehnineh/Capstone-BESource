using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.UpdateParkingSlots;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingSlots
{
    public class UpdateParkingSlotsCommandHandlerTest
    {
        private readonly Mock<IParkingSlotRepository> _parkingSlotRepositoryMock;
        private readonly UpdateParkingSlotsCommandHandler _handler;
        public UpdateParkingSlotsCommandHandlerTest()
        {
            _parkingSlotRepositoryMock = new Mock<IParkingSlotRepository>();
            _handler = new UpdateParkingSlotsCommandHandler(_parkingSlotRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidCommand_ShouldUpdateParkingSlot()
        {
            // Arrange

            var command = new UpdateParkingSlotsCommand
            {
                ParkingSlotId = 1, // Replace with an existing parking slot Id
                RowIndex = 2, // Replace with the new row index
                ColumnIndex = 3 // Replace with the new column index
            };

            var existingSlot = new ParkingSlot
            {
                ParkingSlotId = 1,
                // Add other properties of the existing parking slot
                RowIndex = 1,
                ColumnIndex = 1
            };

            _parkingSlotRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingSlot);



            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");

            // Additional assertions to verify the update
            _parkingSlotRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _parkingSlotRepositoryMock.Verify(repo => repo.Update(It.IsAny<ParkingSlot>()), Times.Once);
        }
        [Fact]
        public async Task Handle_SlotNotFound_ShouldReturnBadRequest()
        {

            var command = new UpdateParkingSlotsCommand
            {
                ParkingSlotId = 1 // Replace with a non-existing parking slot Id
                                  // Add other properties as needed for testing
            };

            _parkingSlotRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((ParkingSlot)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy slot.");
            result.Data.ShouldBe(default(string));

            // Additional assertions to verify the update
            _parkingSlotRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _parkingSlotRepositoryMock.Verify(repo => repo.Update(It.IsAny<ParkingSlot>()), Times.Never);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {
            // Arrange

            var command = new UpdateParkingSlotsCommand
            {
                ParkingSlotId = 1 // Replace with an existing parking slot Id
                                  // Add other properties as needed for testing
            };

            var existingSlot = new ParkingSlot
            {
                ParkingSlotId = 1,
                // Add other properties of the existing parking slot
                RowIndex = 1,
                ColumnIndex = 1
            };

            _parkingSlotRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(existingSlot);
            _parkingSlotRepositoryMock.Setup(repo => repo.Update(It.IsAny<ParkingSlot>())).Throws(new Exception("Simulated exception"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));
            // You can also check the specific exception message if needed.

            // Additional assertions to verify the update
            _parkingSlotRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _parkingSlotRepositoryMock.Verify(repo => repo.Update(It.IsAny<ParkingSlot>()), Times.Once);
        }
    }
}
