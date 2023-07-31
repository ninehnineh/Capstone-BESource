using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.ParkingManagement.Queries.GetAllParkingForAdmin;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetListTraffic;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.ParkingManagement
{
    public class GetAllParkingForAdminQueryHandlerTest
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetAllParkingForAdminQueryHandler _handler;
        public GetAllParkingForAdminQueryHandlerTest()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new GetAllParkingForAdminQueryHandler(_parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetAllParkingForAdminQuery
            {
                PageNo = 1,
                PageSize = 10
            };

            var parkings = new List<Domain.Entities.Parking>
            {
                new Domain.Entities.Parking
                {
                    ParkingId = 1,
                    Name = "Bx1",
                    IsActive = true,
                },
                new Domain.Entities.Parking
                {
                    ParkingId = 2,
                    Name = "Bx2",
                    IsActive = true,
                }
            };
            _parkingRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, x => x.ParkingId, true, request.PageNo, request.PageSize)).ReturnsAsync(parkings);

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
            var request = new GetAllParkingForAdminQuery
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
            var request = new GetAllParkingForAdminQuery
            {
                PageNo = 1000,
                PageSize = 10
            };
            _parkingRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, x => x.ParkingId, true, request.PageNo, request.PageSize)).ReturnsAsync((List<Domain.Entities.Parking>)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
        }
    }
}
