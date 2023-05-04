using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloor;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Floors.FloorManagement
{
    public class GetListFloorQueryHandlerTests
    {
        private readonly Mock<IFloorRepository> _floorRepositoryMock;
        private readonly GetListFloorQueryHandler _handler;
        public GetListFloorQueryHandlerTests()
        {
            _floorRepositoryMock = new Mock<IFloorRepository>();
            _handler = new GetListFloorQueryHandler(_floorRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetListFloorQuery
            {
                PageNo = 1,
                PageSize = 10
            };

            var floors = new List<Floor>
            {
                new Floor
                {
                    FloorId = 1,
                    FloorName = "Tầng 1",
                    IsActive = true,
                    ParkingId = 5
                },
                new Floor
                {
                    FloorId = 2,
                    FloorName = "Tầng 2",
                    IsActive = true,
                    ParkingId = 5
                }
            };
            _floorRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Floor, bool>>>(), null, null, true, request.PageNo, request.PageSize)).ReturnsAsync(floors);

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
            var request = new GetListFloorQuery
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
        public async Task Handle_WithNoFloorFound_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetListFloorQuery
            {
                PageNo = 1000,
                PageSize = 10
            };
            _floorRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Floor, bool>>>(), null, null, true, request.PageNo, request.PageSize)).ReturnsAsync((List<Floor>)null);

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
