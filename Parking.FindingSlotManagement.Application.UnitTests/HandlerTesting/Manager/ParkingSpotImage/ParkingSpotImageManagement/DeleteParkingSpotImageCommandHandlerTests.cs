using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.DeleteParkingSpotImage;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingSpotImage.ParkingSpotImageManagement
{
    public class DeleteParkingSpotImageCommandHandlerTests
    {
        private readonly Mock<IParkingSpotImageRepository> _parkingSpotImageRepositoryMock;
        private readonly DeleteParkingSpotImageCommandHandler _handler;
        public DeleteParkingSpotImageCommandHandlerTests()
        {
            _parkingSpotImageRepositoryMock = new Mock<IParkingSpotImageRepository>();
            _handler = new DeleteParkingSpotImageCommandHandler(_parkingSpotImageRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_DeleteParkingSpotImage_When_Exist()
        {
            // Arrange
            var command = new DeleteParkingSpotImageCommand { ParkingSpotImageId = 1 };
            var parkingSpotImageExist = new Domain.Entities.ParkingSpotImage
            {
                ParkingSpotImageId = 1,
                ImgPath = "https://i.imgur.com/q0Hm688.jpg",
                ParkingId = 5
            };
            _parkingSpotImageRepositoryMock.Setup(x => x.GetById(command.ParkingSpotImageId)).ReturnsAsync(parkingSpotImageExist);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");

            _parkingSpotImageRepositoryMock.Verify(x => x.Delete(parkingSpotImageExist), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_ReturnNotFound_When_NotExist()
        {
            // Arrange
            var command = new DeleteParkingSpotImageCommand { ParkingSpotImageId = 1000000 };
            _parkingSpotImageRepositoryMock.Setup(x => x.GetById(command.ParkingSpotImageId)).ReturnsAsync((Domain.Entities.ParkingSpotImage)null);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy.");

            _parkingSpotImageRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain.Entities.ParkingSpotImage>()), Times.Never);
        }
    }
}
