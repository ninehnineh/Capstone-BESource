using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.DisableOrEnableParkingPrice;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingPrice
{
    public class DisableOrEnableParkingPriceCommandHandlerTest
    {
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepository;
        private readonly DisableOrEnableParkingPriceCommandHandler _handler;
        public DisableOrEnableParkingPriceCommandHandlerTest()
        {
            _parkingPriceRepository = new Mock<IParkingPriceRepository>();
            _handler = new DisableOrEnableParkingPriceCommandHandler(_parkingPriceRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_Return_Successful_Response()
        {
            // Arrange
            var request = new DisableOrEnableParkingPriceCommand
            {
                ParkingPriceId = 1
            };

            var parkingPriceToDelete = new Domain.Entities.ParkingPrice
            {
                ParkingPriceId = request.ParkingPriceId
            };

            _parkingPriceRepository.Setup(x => x.GetById(request.ParkingPriceId))
                .ReturnsAsync(parkingPriceToDelete);

            _parkingPriceRepository.Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Count.ShouldBe(0);

            _parkingPriceRepository.Verify(x => x.GetById(request.ParkingPriceId), Times.Once);
            _parkingPriceRepository.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_Response_When_Floor_Not_Found()
        {
            // Arrange
            var request = new DisableOrEnableParkingPriceCommand
            {
                ParkingPriceId = 99999
            };

            _parkingPriceRepository.Setup(x => x.GetById(request.ParkingPriceId))
                .ReturnsAsync((Domain.Entities.ParkingPrice)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy gói.");
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Count.ShouldBe(0);

            _parkingPriceRepository.Verify(x => x.GetById(request.ParkingPriceId), Times.Once);
            _parkingPriceRepository.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_Should_Return_Error_Response_When_Exception_Occurs()
        {
            // Arrange
            var request = new DisableOrEnableParkingPriceCommand
            {
                ParkingPriceId = 1
            };

            _parkingPriceRepository.Setup(x => x.GetById(request.ParkingPriceId))
                .ThrowsAsync(new Exception("Some error message"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            _parkingPriceRepository.Verify(x => x.GetById(request.ParkingPriceId), Times.Once);
            _parkingPriceRepository.Verify(x => x.Save(), Times.Never);
        }
    }
}
