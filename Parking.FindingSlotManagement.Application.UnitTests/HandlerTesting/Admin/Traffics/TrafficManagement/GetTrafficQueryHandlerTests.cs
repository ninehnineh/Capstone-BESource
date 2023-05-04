using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetTraffic;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Traffics.TrafficManagement
{
    public class GetTrafficQueryHandlerTests
    {
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly GetTrafficQueryHandler _handler;
        public GetTrafficQueryHandlerTests()
        {
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _handler = new GetTrafficQueryHandler(_trafficRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var query = new GetTrafficQuery()
            {
                TrafficId = 1
            };
            var traffic = new Traffic
            {
                TrafficId = 1,
                Name = "Xe ô tô",
                IsActive = true
            };
            _trafficRepositoryMock.Setup(x => x.GetById(query.TrafficId)).ReturnsAsync(traffic);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_WithInvalidId_ReturnsNotFound()
        {
            var query = new GetTrafficQuery()
            {
                TrafficId = 9999999
            };
            _trafficRepositoryMock.Setup(x => x.GetById(query.TrafficId)).ReturnsAsync((Traffic)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy.");
        }
    }
}
