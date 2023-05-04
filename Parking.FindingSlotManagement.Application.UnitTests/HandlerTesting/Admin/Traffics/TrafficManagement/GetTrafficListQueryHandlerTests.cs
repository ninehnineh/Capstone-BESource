using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetListTraffic;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Traffics.TrafficManagement
{
    public class GetTrafficListQueryHandlerTests
    {
        private readonly Mock<ITrafficRepository> _trafficRepositoryMock;
        private readonly GetTrafficListQueryHandler _handler;
        public GetTrafficListQueryHandlerTests()
        {
            _trafficRepositoryMock = new Mock<ITrafficRepository>();
            _handler = new GetTrafficListQueryHandler(_trafficRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetTrafficListQuery
            {
                PageNo = 1,
                PageSize = 10
            };

            var traffics = new List<Traffic>
            {
                new Traffic
                {
                    TrafficId = 1,
                    Name = "Xe ô tô",
                    IsActive = true,
                },
                new Traffic
                {
                    TrafficId = 2,
                    Name = "Xe máy",
                    IsActive = true,
                }
            };
            _trafficRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Traffic, bool>>>(), null, x => x.TrafficId, true, request.PageNo, request.PageSize)).ReturnsAsync(traffics);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
        }
        [Fact]
        public async Task Handle_WithInvalidPageNo_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetTrafficListQuery
            {
                PageNo = 0,
                PageSize = 10
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
        }
        [Fact]
        public async Task Handle_WithNoTRafficFound_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetTrafficListQuery
            {
                PageNo = 1000,
                PageSize = 10
            };
            _trafficRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Traffic, bool>>>(), null, x => x.TrafficId, true, request.PageNo, request.PageSize)).ReturnsAsync((List<Traffic>)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy");
        }
    }
}
