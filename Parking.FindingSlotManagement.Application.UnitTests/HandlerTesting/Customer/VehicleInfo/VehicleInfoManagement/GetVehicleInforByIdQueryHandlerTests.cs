using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Queries.GetVehicleInforById;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.VehicleInfo.VehicleInfoManagement
{
    public class GetVehicleInforByIdQueryHandlerTests
    {
        private readonly Mock<IVehicleInfoRepository> _vehicleInforRepository;
        private readonly GetVehicleInforByIdQueryHandler _handler;
        public GetVehicleInforByIdQueryHandlerTests()
        {
            _vehicleInforRepository = new Mock<IVehicleInfoRepository>();
            _handler = new GetVehicleInforByIdQueryHandler(_vehicleInforRepository.Object);
        }
        [Fact]
        public async Task Handle_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var query = new GetVehicleInforByIdQuery()
            {
                VehicleInforId = 1
            };
            var vehicleInfor = new VehicleInfor
            {
                VehicleInforId = 1,
                LicensePlate = "51G-678.89",
                VehicleName = "Mercedes G63",
                Color = "Black",
            };
            _vehicleInforRepository.Setup(x => x.GetById(query.VehicleInforId)).ReturnsAsync(vehicleInfor);

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
            var query = new GetVehicleInforByIdQuery()
            {
                VehicleInforId = 9999999
            };
            _vehicleInforRepository.Setup(x => x.GetById(query.VehicleInforId)).ReturnsAsync((VehicleInfor)null);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy thông tin phương tiện.");
        }
    }
}
