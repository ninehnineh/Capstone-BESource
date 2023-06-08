using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.DeleteParkingHasPrice;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingHasPrice.ParkingHasPriceManagement
{
    public class DeleteParkingHasPriceCommandHandlerTests
    {
        private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;
        private readonly DeleteParkingHasPriceCommandHandler _handler;
        public DeleteParkingHasPriceCommandHandlerTests()
        {
            _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
            _handler = new DeleteParkingHasPriceCommandHandler(_parkingHasPriceRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_DeleteParkingHasPriceCommand_Should_Return_SuccessfulResponse()
        {
            //Arrange
            var parkingHasPriceExist = new Domain.Entities.ParkingHasPrice()
            {
                ParkingHasPriceId = 1,
                ParkingId = 1,
                ParkingPriceId = 1
            };
            var command = new DeleteParkingHasPriceCommand { ParkingHasPriceId = 1 };
            _parkingHasPriceRepositoryMock.Setup(x => x.GetById(command.ParkingHasPriceId)).ReturnsAsync(parkingHasPriceExist);
            _parkingHasPriceRepositoryMock.Setup(x => x.Delete(parkingHasPriceExist)).Returns(Task.CompletedTask);


            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Message.ShouldBe("Thành công");
            _parkingHasPriceRepositoryMock.Verify(x => x.GetById(parkingHasPriceExist.ParkingHasPriceId), Times.Once);
            _parkingHasPriceRepositoryMock.Verify(x => x.Delete(parkingHasPriceExist), Times.Once);

        }
        [Fact]
        public async Task Handle_WithNonExistingParkingHasPrice_ShouldReturnErrorResponse()
        {
            // Arrange
            var ParkingHasPriceId = 10000;
            var request = new DeleteParkingHasPriceCommand { ParkingHasPriceId = 1 };
            var cancellationToken = CancellationToken.None;
            _parkingHasPriceRepositoryMock.Setup(x => x.GetById(ParkingHasPriceId)).ReturnsAsync((Domain.Entities.ParkingHasPrice)null);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tồn tại");
            _parkingHasPriceRepositoryMock.Verify(x => x.GetById(ParkingHasPriceId), Times.Never);

        }
    }
}
