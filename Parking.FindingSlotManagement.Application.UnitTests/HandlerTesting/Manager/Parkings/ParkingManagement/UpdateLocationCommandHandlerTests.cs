using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.UpdateLocationOfParking;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Parkings.ParkingManagement
{
    public class UpdateLocationCommandHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly UpdateLocationCommandHandler _handler;
        private readonly UpdateLocationCommandValidation _validator;
        public UpdateLocationCommandHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _validator = new UpdateLocationCommandValidation();
            _handler = new UpdateLocationCommandHandler(_parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenParkingExists()
        {
            // Arrange
            var parkingId = 1;
            var request = new UpdateLocationCommand { ParkingId = parkingId, Latitude = (decimal?)1.0, Longitude = (decimal?)2.0 };
            var existingParking = new Domain.Entities.Parking { ParkingId = parkingId };

            _parkingRepositoryMock.Setup(x => x.GetById(parkingId))
                .ReturnsAsync(existingParking);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Thành công");

            existingParking.Latitude.ShouldBe(request.Latitude);
            existingParking.Longitude.ShouldBe(request.Longitude);

            _parkingRepositoryMock.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldReturnNotFoundResponse_WhenParkingDoesNotExist()
        {
            // Arrange
            var parkingId = 1;
            var request = new UpdateLocationCommand { ParkingId = parkingId, Latitude = (decimal?)1.0, Longitude = (decimal?)2.0 };

            _parkingRepositoryMock.Setup(x => x.GetById(parkingId))
                .ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy.");

            _parkingRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public void Latitude_ShouldNotBeEmpty()
        {
            var command = new UpdateLocationCommand
            {
                ParkingId = 1,
                Latitude = null,
                Longitude = (decimal?)2.0
            };
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Latitude);
        }
        [Fact]
        public void Latitude_ShouldNotBeNull()
        {
            var command = new UpdateLocationCommand()
            {
                ParkingId = 1,
                Longitude = (decimal?)2.0
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Latitude);
        }
        [Fact]
        public void Longitude_ShouldNotBeEmpty()
        {
            var command = new UpdateLocationCommand
            {
                ParkingId = 1,
                Latitude = (decimal?)2.0,
                Longitude = null
            };
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Longitude);
        }
        [Fact]
        public void Longitude_ShouldNotBeNull()
        {
            var command = new UpdateLocationCommand()
            {
                ParkingId = 1,
                Latitude = (decimal?)2.0
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Longitude);
        }
    }
}
