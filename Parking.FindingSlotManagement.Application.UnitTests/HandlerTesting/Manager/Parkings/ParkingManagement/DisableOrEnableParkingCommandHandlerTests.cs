using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.DisableOrEnableParking;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Parkings.ParkingManagement
{
    public class DisableOrEnableParkingCommandHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly DisableOrEnableParkingCommandHandler _handler;
        public DisableOrEnableParkingCommandHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new DisableOrEnableParkingCommandHandler(_parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_Return_Successful_Response()
        {
            // Arrange
            var request = new DisableOrEnableParkingCommand
            {
                ParkingId = 5
            };

            var parkingToDelete = new Domain.Entities.Parking
            {
                ParkingId = request.ParkingId,
                IsActive = true,
            };

            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync(parkingToDelete);

            _parkingRepositoryMock.Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Count.ShouldBe(0);

            _parkingRepositoryMock.Verify(x => x.GetById(request.ParkingId), Times.Once);
            _parkingRepositoryMock.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_Response_When_Parking_Not_Found()
        {
            // Arrange
            var request = new DisableOrEnableParkingCommand
            {
                ParkingId = 1
            };

            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy bãi.");
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Count.ShouldBe(0);

            _parkingRepositoryMock.Verify(x => x.GetById(request.ParkingId), Times.Once);
            _parkingRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_Should_Return_Error_Response_When_Exception_Occurs()
        {
            // Arrange
            var request = new DisableOrEnableParkingCommand
            {
                ParkingId = 5
            };

            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ThrowsAsync(new Exception("Some error message"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            _parkingRepositoryMock.Verify(x => x.GetById(request.ParkingId), Times.Once);
            _parkingRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
    }
}
