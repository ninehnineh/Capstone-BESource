using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Queries.GetListVehicleInforByUserId;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.VehicleInfo.VehicleInfoManagement
{
    public class GetListVehicleInforByUserIdQueryHandlerTests
    {
        private readonly Mock<IVehicleInfoRepository> _vehicleInforRepositoryMock;
        private readonly GetListVehicleInforByUserIdQueryHandler _handler;
        public GetListVehicleInforByUserIdQueryHandlerTests()
        {
            _vehicleInforRepositoryMock = new Mock<IVehicleInfoRepository>();
            _handler = new GetListVehicleInforByUserIdQueryHandler(_vehicleInforRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ReturnsServiceResponseWithSuccessTrue()
        {
            // Arrange
            var request = new GetListVehicleInforByUserIdQuery
            {
                UserId = 10,
                PageNo = 1,
                PageSize = 10
            };

            var vehicleInforList = new List<VehicleInfor>
            {
                new VehicleInfor
                {
                    VehicleInforId = 1,
                    LicensePlate = "51G-678.89",
                    VehicleName = "Mercedes G63",
                    Color = "Black",
                    UserId = 10,
                    TrafficId = 1
                },

            };
            _vehicleInforRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<VehicleInfor, bool>>>(), It.IsAny<List<Expression<Func<VehicleInfor, object>>>>(), null, true, request.PageNo, request.PageSize)).ReturnsAsync(vehicleInforList);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công.");
        }
        [Fact]
        public async Task Handle_WithInvalidPageNo_ReturnsServiceResponseWithSuccessTrueAndCountZero()
        {
            // Arrange
            var request = new GetListVehicleInforByUserIdQuery
            {
                UserId = 10,
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
            var request = new GetListVehicleInforByUserIdQuery
            {
                UserId = 10,
                PageNo = 1000,
                PageSize = 10
            };
            _vehicleInforRepositoryMock.Setup(x => x.GetAllItemWithPagination(It.IsAny<Expression<Func<VehicleInfor, bool>>>(), It.IsAny<List<Expression<Func<VehicleInfor, object>>>>(), null, true, request.PageNo, request.PageSize)).ReturnsAsync((List<VehicleInfor>)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tìm thấy.");
        }
    }
}
