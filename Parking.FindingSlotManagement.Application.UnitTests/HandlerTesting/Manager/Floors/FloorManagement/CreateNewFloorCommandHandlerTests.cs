using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.CreateNewFloor;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Floors.FloorManagement
{
    public class CreateNewFloorCommandHandlerTests
    {
        private readonly Mock<IFloorRepository> _floorRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly CreateNewFloorCommandHandler _handler;
        private readonly CreateNewFloorCommandValidation _validator;
        public CreateNewFloorCommandHandlerTests()
        {
            _floorRepositoryMock = new Mock<IFloorRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _validator = new CreateNewFloorCommandValidation();
            _handler = new CreateNewFloorCommandHandler(_floorRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewFloorCommand
            {
                FloorName = "Tầng 3",
                ParkingId = 5
            };

            var parkingExist = new Domain.Entities.Parking { ParkingId = 5 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(parkingExist);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            _floorRepositoryMock.Verify(x => x.Insert(It.IsAny<Floor>()), Times.Once);
        }
        [Fact]
        public async Task Handle_InvalidParkingId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new CreateNewFloorCommand
            {
                FloorName = "Tầng 3",
                ParkingId = 1
            };

            _parkingRepositoryMock.Setup(x => x.GetById(command.ParkingId)).ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy bãi xe.");
            result.StatusCode.ShouldBe(200);

            _floorRepositoryMock.Verify(x => x.Insert(It.IsAny<Floor>()), Times.Never);
        }
        [Fact]
        public void FloorName_ShouldNotBeEmpty()
        {
            var command = new CreateNewFloorCommand
            {
                FloorName = "",
                ParkingId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FloorName);
        }
        [Fact]
        public void FloorName_ShouldNotBeNull()
        {
            var command = new CreateNewFloorCommand
            {
                ParkingId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FloorName);
        }
        [Fact]
        public void FloorName_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewFloorCommand
            {
                FloorName = "Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3Tầng 3",
                ParkingId = 1
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FloorName);
        }
        [Fact]
        public void ParkingId_ShouldNotBeEmpty()
        {
            var command = new CreateNewFloorCommand
            {
                FloorName = "Tầng 3",
                ParkingId = null
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
        }
        [Fact]
        public void ParkingId_ShouldNotBeNull()
        {
            var command = new CreateNewFloorCommand
            {
                FloorName = "Tầng 3"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
        }
    }
}
