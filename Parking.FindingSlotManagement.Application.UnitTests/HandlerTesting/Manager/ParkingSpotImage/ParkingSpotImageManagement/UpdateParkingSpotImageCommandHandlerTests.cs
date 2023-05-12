using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.UpdateParkingSpotImage;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingSpotImage.ParkingSpotImageManagement
{
    public class UpdateParkingSpotImageCommandHandlerTests
    {
        private readonly Mock<IParkingSpotImageRepository> _parkingSpotImageRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly UpdateParkingSpotImageCommandValidation _validator;
        private readonly UpdateParkingSpotImageCommandHandler _handler;
        public UpdateParkingSpotImageCommandHandlerTests()
        {
            _parkingSpotImageRepositoryMock = new Mock<IParkingSpotImageRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _validator = new UpdateParkingSpotImageCommandValidation();
            _handler = new UpdateParkingSpotImageCommandHandler(_parkingSpotImageRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateParkingSpotImageCommandHandler_Should_Update_ParkingSpotImage_Successfully()
        {
            // Arrange
            var request = new UpdateParkingSpotImageCommand
            {
                ParkingSpotImageId = 1,
                ParkingId = 5,
                ImgPath = "string"
            };
            var cancellationToken = new CancellationToken();
            var OldParkingSpotImage = new Domain.Entities.ParkingSpotImage
            {
                ParkingSpotImageId = 1,
                ImgPath = "https://i.imgur.com/q0Hm688.jpg",
                ParkingId = 5
            };

            _parkingSpotImageRepositoryMock.Setup(x => x.GetById(request.ParkingSpotImageId))
                .ReturnsAsync(OldParkingSpotImage);
            var checkParkingExist = new Domain.Entities.Parking { ParkingId = 5 };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync(checkParkingExist);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldParkingSpotImage.ImgPath.ShouldBe(request.ImgPath);
            OldParkingSpotImage.ParkingId.ShouldBe(request.ParkingId);
            _parkingSpotImageRepositoryMock.Verify(x => x.Update(OldParkingSpotImage), Times.Once);
        }
        [Fact]
        public async Task UpdateParkingSpotImageCommandHandler_Should_Return_Not_Found_If_ParkingSpotImage_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateParkingSpotImageCommand
            {
                ParkingSpotImageId = 2000
            };
            var cancellationToken = new CancellationToken();
            _parkingSpotImageRepositoryMock.Setup(x => x.GetById(request.ParkingSpotImageId))
                .ReturnsAsync((Domain.Entities.ParkingSpotImage)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _parkingSpotImageRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ParkingSpotImage>()), Times.Never);
        }
        [Fact]
        public async Task UpdateParkingSpotImageCommandHandler_Should_Return_Not_Found_If_Parking_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateParkingSpotImageCommand
            {
                ParkingSpotImageId = 1,
                ParkingId = 5,
                ImgPath = "string"
            };
            var cancellationToken = new CancellationToken();
            var OldParkingSpotImage = new Domain.Entities.ParkingSpotImage
            {
                ParkingSpotImageId = 1,
                ImgPath = "https://i.imgur.com/q0Hm688.jpg",
                ParkingId = 5
            };

            _parkingSpotImageRepositoryMock.Setup(x => x.GetById(request.ParkingSpotImageId))
                .ReturnsAsync(OldParkingSpotImage);
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId)).ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _parkingSpotImageRepositoryMock.Verify(x => x.Update(It.IsAny<Domain.Entities.ParkingSpotImage>()), Times.Never);
        }
        [Fact]
        public void ImgPath_ShouldNotExceedMaximumLength()
        {
            var command = new UpdateParkingSpotImageCommand
            {
                ParkingSpotImageId = 1,
                ParkingId = 5,
                ImgPath = "stringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstringstring"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ImgPath);
        }
    }
}
