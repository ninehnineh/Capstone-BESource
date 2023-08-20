using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetParkingById;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Parkings.ParkingManagement
{
    public class GetParkingByIdQueryHandlerTests
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IFloorRepository> _floorRepositoryMock;
        private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;
        private readonly GetParkingByIdQueryHandler _handler;
        public GetParkingByIdQueryHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
            _floorRepositoryMock = new Mock<IFloorRepository>();
            _handler = new GetParkingByIdQueryHandler(_parkingRepositoryMock.Object, _floorRepositoryMock.Object, _parkingHasPriceRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ParkingNotFound_ShouldReturnNotFound()
        {
            // Arrange
            int parkingId = 1; // Replace with a non-existing parkingId

            var mockParkingRepository = new Mock<IParkingRepository>();
            var mockFloorRepository = new Mock<IFloorRepository>();
            var mockParkingHasPriceRepository = new Mock<IParkingHasPriceRepository>();

            mockParkingRepository.Setup(repo => repo.GetById(parkingId)).ReturnsAsync((Domain.Entities.Parking)null);

            var handler = new GetParkingByIdQueryHandler(
                mockParkingRepository.Object,
                mockFloorRepository.Object,
                mockParkingHasPriceRepository.Object);

            var query = new GetParkingByIdQuery
            {
                ParkingId = parkingId
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
        }
        /*[Fact]
        public async Task Handle_InactiveParking_ShouldReturnInactive()
        {
            // Arrange
            int parkingId = 1; // Replace with an existing parkingId

            var mockParkingRepository = new Mock<IParkingRepository>();
            var mockFloorRepository = new Mock<IFloorRepository>();
            var mockParkingHasPriceRepository = new Mock<IParkingHasPriceRepository>();

            var parking = new Domain.Entities.Parking
            {
                ParkingId = parkingId,
                IsActive = false
            };

            mockParkingRepository.Setup(repo => repo.GetById(parkingId)).ReturnsAsync(parking);

            var handler = new GetParkingByIdQueryHandler(
                mockParkingRepository.Object,
                mockFloorRepository.Object,
                mockParkingHasPriceRepository.Object);

            var query = new GetParkingByIdQuery
            {
                ParkingId = parkingId
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Bãi đã bị cấm.");
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
        }*/
        [Fact]
        public async Task Handle_ParkingFound_ShouldReturnParkingInfo()
        {
            // Arrange
            int parkingId = 1; // Replace with an existing parkingId



            var parking = new Domain.Entities.Parking
            {
                ParkingId = parkingId,
                IsActive = true
                // Add other properties as needed for testing
            };

            var floors = new List<Floor>
            {
                new Floor { FloorId = 1, ParkingId = parkingId, IsActive = true },
                new Floor { FloorId = 2, ParkingId = parkingId, IsActive = true }
                // Add more floors as needed for testing
            };

            var parkingHasPrices = new List<Domain.Entities.ParkingHasPrice>
            {
                new Domain.Entities.ParkingHasPrice { ParkingHasPriceId = 1, ParkingId = parkingId },
                new Domain.Entities.ParkingHasPrice { ParkingHasPriceId = 2, ParkingId = parkingId }
                // Add more parkingHasPrices as needed for testing
            };

            _parkingRepositoryMock.Setup(repo => repo.GetById(parkingId)).ReturnsAsync(parking);
            _floorRepositoryMock.Setup(repo => repo.GetAllItemWithConditionByNoInclude(
                It.IsAny<Expression<Func<Floor, bool>>>())).ReturnsAsync(floors);
            _parkingHasPriceRepositoryMock.Setup(repo => repo.GetAllItemWithConditionByNoInclude(
                It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>())).ReturnsAsync(parkingHasPrices);


            var query = new GetParkingByIdQuery
            {
                ParkingId = parkingId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            result.Count.ShouldBe(1);

            // Additional assertions based on the returned data
            result.Data.ParkingEntity.ShouldNotBeNull();
            result.Data.NumberOfFloors.ShouldBe(floors.Count);
            result.Data.NumberofParkingHasPrice.ShouldBe(parkingHasPrices.Count);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {
            // Arrange
            int parkingId = 1; // Replace with an existing parkingId


            _parkingRepositoryMock.Setup(repo => repo.GetById(parkingId)).Throws(new Exception("Simulated exception"));

            var query = new GetParkingByIdQuery
            {
                ParkingId = parkingId
            };

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await _handler.Handle(query, CancellationToken.None));
            // You can also check the specific exception message if needed.
        }
    }
}
