using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.CreateNewParkingSpotImage;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingSpotImage.ParkingSpotImageManagement
{
    public class CreateNewParkingSpotImageCommandHandlerTests
    {
        private readonly Mock<IParkingSpotImageRepository> _parkingSpotImageRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly CreateNewParkingSpotImageCommandValidation _validator;
        private readonly CreateNewParkingSpotImageCommandHandler _handler;
        public CreateNewParkingSpotImageCommandHandlerTests()
        {
            _parkingSpotImageRepositoryMock = new Mock<IParkingSpotImageRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _validator = new CreateNewParkingSpotImageCommandValidation();
            _handler = new CreateNewParkingSpotImageCommandHandler(_parkingSpotImageRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenCreatedSuccessfully_ReturnsCreated()
        {
            // Arrange
            var request = new CreateNewParkingSpotImageCommand
            {
                ImgPath = "https://i.imgur.com/q0Hm688.jpg",
                ParkingId = 5
            };
            var expectedParkingSpotImage = new Domain.Entities.ParkingSpotImage
            {
                ParkingSpotImageId = 1,
                ImgPath = "https://i.imgur.com/q0Hm688.jpg",
                ParkingId = 5
            };
            var parkingExist = new Domain.Entities.Parking
            {
                ParkingId = 5
            };
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync(parkingExist);


            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(201);
            response.Success.ShouldBeTrue();
            response.Count.ShouldBe(0);
            response.Message.ShouldBe("Thành công");
            // Verify that the account repository was called to insert the new account
            _parkingSpotImageRepositoryMock.Verify(x => x.Insert(It.Is<Domain.Entities.ParkingSpotImage>(parkingImage => parkingImage.ImgPath == expectedParkingSpotImage.ImgPath)));
        }
        [Fact]
        public async Task Handle_Parking_Does_Not_Exists_ReturnsErrorResponse()
        {
            // Arrange
            var command = new CreateNewParkingSpotImageCommand
            {
                ImgPath = "https://i.imgur.com/q0Hm688.jpg",
                ParkingId = 5
            };
            var expectedParkingSpotImage = new Domain.Entities.ParkingSpotImage
            {
                ParkingSpotImageId = 1,
                ImgPath = "https://i.imgur.com/q0Hm688.jpg",
                ParkingId = 5
            };
            _parkingRepositoryMock.Setup(x => x.GetById(command.ParkingId))
                .ReturnsAsync((Domain.Entities.Parking)null);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Data.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            result.StatusCode.ShouldBe(200);

            _parkingSpotImageRepositoryMock.Verify(x => x.Insert(It.IsAny<Domain.Entities.ParkingSpotImage>()), Times.Never);
        }
        [Fact]
        public void ImgPath_ShouldNotBeEmpty()
        {
            var command = new CreateNewParkingSpotImageCommand
            {
                ImgPath = "",
                ParkingId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ImgPath);
        }
        [Fact]
        public void ImgPath_ShouldNotBeNull()
        {
            var command = new CreateNewParkingSpotImageCommand
            {
                ParkingId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ImgPath);
        }
        [Fact]
        public void ImgPath_ShouldNotExceedMaximumLength()
        {
            var command = new CreateNewParkingSpotImageCommand
            {
                ImgPath = "https://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpg.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpg.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpg.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpg.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpghttps://i.imgur.com/q0Hm688.jpg",
                ParkingId = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ImgPath);
        }
        [Fact]
        public void ParkingId_ShouldNotBeEmpty()
        {
            var command = new CreateNewParkingSpotImageCommand
            {
                ImgPath = "https://i.imgur.com/q0Hm688.jpg",
                ParkingId = null
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
        }
        [Fact]
        public void ParkingId_ShouldNotBeNull()
        {
            var command = new CreateNewParkingSpotImageCommand
            {
                ImgPath = "https://i.imgur.com/q0Hm688.jpg"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
        }
    }
}
