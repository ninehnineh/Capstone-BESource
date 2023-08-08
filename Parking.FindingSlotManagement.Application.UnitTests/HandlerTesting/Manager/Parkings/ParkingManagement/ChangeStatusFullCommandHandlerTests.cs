using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.ChangeStatusFull;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Parkings.ParkingManagement
{
    public class ChangeStatusFullCommandHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly ChangeStatusFullCommandHandler _handler;
        public ChangeStatusFullCommandHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new ChangeStatusFullCommandHandler(_parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ChangeIsFullStatus_FromFalseToTrue_ShouldReturnSuccess()
        {
            // Arrange
            int parkingId = 1; // Replace with a valid parkingId

            var mockParkingRepository = new Mock<IParkingRepository>();

            var parking = new Domain.Entities.Parking
            {
                ParkingId = parkingId,
                IsActive = true,
                IsFull = false
            };

            mockParkingRepository.Setup(repo => repo.GetById(parkingId)).ReturnsAsync(parking);

            var handler = new ChangeStatusFullCommandHandler(mockParkingRepository.Object);
            var command = new ChangeStatusFullCommand
            {
                ParkingId = parkingId
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");
            mockParkingRepository.Verify(repo => repo.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_ChangeIsFullStatus_FromTrueToFalse_ShouldReturnSuccess()
        {
            // Arrange
            int parkingId = 1; // Replace with a valid parkingId

            var mockParkingRepository = new Mock<IParkingRepository>();

            var parking = new Domain.Entities.Parking
            {
                ParkingId = parkingId,
                IsActive = true,
                IsFull = true
            };

            mockParkingRepository.Setup(repo => repo.GetById(parkingId)).ReturnsAsync(parking);

            var handler = new ChangeStatusFullCommandHandler(mockParkingRepository.Object);
            var command = new ChangeStatusFullCommand
            {
                ParkingId = parkingId
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");
            mockParkingRepository.Verify(repo => repo.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_NonExistingParking_ShouldReturnNotFound()
        {
            // Arrange
            int parkingId = 1; // Replace with a non-existing parkingId

            var mockParkingRepository = new Mock<IParkingRepository>();

            mockParkingRepository.Setup(repo => repo.GetById(parkingId)).ReturnsAsync((Domain.Entities.Parking)null);

            var handler = new ChangeStatusFullCommandHandler(mockParkingRepository.Object);
            var command = new ChangeStatusFullCommand
            {
                ParkingId = parkingId
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            mockParkingRepository.Verify(repo => repo.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_InactiveParking_ShouldReturnBadRequest()
        {
            // Arrange
            int parkingId = 1; // Replace with a valid parkingId

            var mockParkingRepository = new Mock<IParkingRepository>();

            var parking = new Domain.Entities.Parking
            {
                ParkingId = parkingId,
                IsActive = false,
                IsFull = false
            };

            mockParkingRepository.Setup(repo => repo.GetById(parkingId)).ReturnsAsync(parking);

            var handler = new ChangeStatusFullCommandHandler(mockParkingRepository.Object);
            var command = new ChangeStatusFullCommand
            {
                ParkingId = parkingId
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Bãi giữ xe đã bị vô hiệu hóa.");
            mockParkingRepository.Verify(repo => repo.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {
            // Arrange
            int parkingId = 1; // Replace with a valid parkingId

            var mockParkingRepository = new Mock<IParkingRepository>();

            mockParkingRepository.Setup(repo => repo.GetById(parkingId)).Throws(new Exception("Simulated exception"));

            var handler = new ChangeStatusFullCommandHandler(mockParkingRepository.Object);
            var command = new ChangeStatusFullCommand
            {
                ParkingId = parkingId
            };

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
            // You can also check the specific exception message if needed.
        }
    }
}
