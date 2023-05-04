using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.DisableOrEnableFloor;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Floors.FloorManagement
{
    public class DisableOrEnableFloorCommandHandlerTests
    {
        private readonly Mock<IFloorRepository> _floorRepositoryMock;
        private readonly DisableOrEnableFloorCommandHandler _handler;
        public DisableOrEnableFloorCommandHandlerTests()
        {
            _floorRepositoryMock = new Mock<IFloorRepository>();
            _handler = new DisableOrEnableFloorCommandHandler(_floorRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_Return_Successful_Response()
        {
            // Arrange
            var request = new DisableOrEnableFloorCommand
            {
                FloorId = 1
            };

            var floorToDelete = new Floor
            {
                FloorId = request.FloorId,
                IsActive = true,
            };

            _floorRepositoryMock.Setup(x => x.GetById(request.FloorId))
                .ReturnsAsync(floorToDelete);

            _floorRepositoryMock.Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Count.ShouldBe(0);

            _floorRepositoryMock.Verify(x => x.GetById(request.FloorId), Times.Once);
            _floorRepositoryMock.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_Response_When_Floor_Not_Found()
        {
            // Arrange
            var request = new DisableOrEnableFloorCommand
            {
                FloorId = 999999
            };

            _floorRepositoryMock.Setup(x => x.GetById(request.FloorId))
                .ReturnsAsync((Floor)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy tầng.");
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Count.ShouldBe(0);

            _floorRepositoryMock.Verify(x => x.GetById(request.FloorId), Times.Once);
            _floorRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_Should_Return_Error_Response_When_Exception_Occurs()
        {
            // Arrange
            var request = new DisableOrEnableFloorCommand
            {
                FloorId = 1
            };

            _floorRepositoryMock.Setup(x => x.GetById(request.FloorId))
                .ThrowsAsync(new Exception("Some error message"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            _floorRepositoryMock.Verify(x => x.GetById(request.FloorId), Times.Once);
            _floorRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
    }
}
