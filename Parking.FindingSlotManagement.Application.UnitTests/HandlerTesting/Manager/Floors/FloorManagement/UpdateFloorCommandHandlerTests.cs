using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.UpdateFloor;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Floors.FloorManagement
{
    public class UpdateFloorCommandHandlerTests
    {
        private readonly Mock<IFloorRepository> _floorRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly UpdateFloorCommandValidation _validator;
        private readonly UpdateFloorCommandHandler _handler;
        public UpdateFloorCommandHandlerTests()
        {
            _floorRepositoryMock = new Mock<IFloorRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _validator = new UpdateFloorCommandValidation();
            _handler = new UpdateFloorCommandHandler(_floorRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateFloorCommandHandler_Should_Update_Floor_Successfully()
        {
            // Arrange
            var request = new UpdateFloorCommand
            {
                FloorId = 1,
                FloorName = "Tầng hầm",
                ParkingId = 5
            };
            var cancellationToken = new CancellationToken();
            var OldFloor = new Floor
            {
                FloorId = 1,
                FloorName = "Tầng 1",
                IsActive = true,
                ParkingId = 5
            };
            _floorRepositoryMock.Setup(x => x.GetById(request.FloorId))
                .ReturnsAsync(OldFloor);
            var parkingExist = new Domain.Entities.Parking { ParkingId = 5 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldFloor.FloorName.ShouldBe(request.FloorName);
            OldFloor.ParkingId.ShouldBe(request.ParkingId);
            _floorRepositoryMock.Verify(x => x.Update(OldFloor), Times.Once);
        }
        [Fact]
        public async Task UpdateFloorCommandHandler_Should_Return_Not_Found_If_Floor_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateFloorCommand
            {
                FloorId = 2000
            };
            var cancellationToken = new CancellationToken();
            _floorRepositoryMock.Setup(x => x.GetById(request.FloorId))
                .ReturnsAsync((Floor)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy tầng.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _floorRepositoryMock.Verify(x => x.Update(It.IsAny<Floor>()), Times.Never);
        }
        [Fact]
        public async Task UpdateFloorCommandHandler_Should_Return_Not_Found_If_Parking_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateFloorCommand
            {
                FloorId = 1,
                ParkingId = 1
            };
            var cancellationToken = new CancellationToken();
            var floorExist = new Floor { FloorId = 1 };
            _floorRepositoryMock.Setup(x => x.GetById(request.FloorId))
                .ReturnsAsync(floorExist);
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy bãi xe.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _floorRepositoryMock.Verify(x => x.Update(It.IsAny<Floor>()), Times.Never);
        }
        [Fact]
        public void FloorName_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateFloorCommand
            {
                FloorId = 1,
                FloorName = "Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3",
                ParkingId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FloorName);
        }
    }
}
