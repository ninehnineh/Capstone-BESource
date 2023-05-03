using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.DisableOrEnableTraffic;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Traffics.TrafficManagement
{
    public class DisableOrEnableTrafficCommandHandlerTests
    {
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly DisableOrEnableTrafficCommandHandler _handler;

        public DisableOrEnableTrafficCommandHandlerTests()
        {
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _handler = new DisableOrEnableTrafficCommandHandler(_trafficRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_Should_Return_Successful_Response()
        {
            // Arrange
            var request = new DisableOrEnableTrafficCommand
            {
                TrafficId = 1
            };

            var trafficToDelete = new Traffic
            {
                TrafficId = request.TrafficId,
                IsActive = true,
            };

            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId))
                .ReturnsAsync(trafficToDelete);

            _trafficRepositoryMock.Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            result.Count.ShouldBe(0);

            _trafficRepositoryMock.Verify(x => x.GetById(request.TrafficId), Times.Once);
            _trafficRepositoryMock.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_Return_NotFound_Response_When_Traffic_Not_Found()
        {
            // Arrange
            var request = new DisableOrEnableTrafficCommand
            {
                TrafficId = 15
            };

            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId))
                .ReturnsAsync((Traffic)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy.");
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Count.ShouldBe(0);

            _trafficRepositoryMock.Verify(x => x.GetById(request.TrafficId), Times.Once);
            _trafficRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task Handle_Should_Return_Error_Response_When_Exception_Occurs()
        {
            // Arrange
            var request = new DisableOrEnableTrafficCommand
            {
                TrafficId = 1
            };

            _trafficRepositoryMock.Setup(x => x.GetById(request.TrafficId))
                .ThrowsAsync(new Exception("Some error message"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            _trafficRepositoryMock.Verify(x => x.GetById(request.TrafficId), Times.Once);
            _trafficRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
    }
}
