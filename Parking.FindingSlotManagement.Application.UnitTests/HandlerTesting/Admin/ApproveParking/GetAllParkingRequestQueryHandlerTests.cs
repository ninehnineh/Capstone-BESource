using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetAllParkingRequest;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.ApproveParking
{
    public class GetAllParkingRequestQueryHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetAllParkingRequestQueryHandler _handler;
        public GetAllParkingRequestQueryHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new GetAllParkingRequestQueryHandler(_parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetAllParkingRequestQuery
            {
                PageNo = 1,
                PageSize = 10
            };

            var parkings = new List<Domain.Entities.Parking>
            {
                new Domain.Entities.Parking { ParkingId = 1},
                new Domain.Entities.Parking {ParkingId = 2},
                new Domain.Entities.Parking {ParkingId = 3},
            };
            _parkingRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), x => x.ParkingId, true, request.PageNo, request.PageSize)).ReturnsAsync(parkings);

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
            var request = new GetAllParkingRequestQuery
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
        public async Task Handle_WithNoAccountFound_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetAllParkingRequestQuery
            {
                PageNo = 1000,
                PageSize = 10
            };
            var parkings = new List<Domain.Entities.Parking>();
            _parkingRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.Parking, object>>>>(), x => x.ParkingId, true, request.PageNo, request.PageSize)).ReturnsAsync(parkings);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Danh sách trống.");
        }
    }
}
