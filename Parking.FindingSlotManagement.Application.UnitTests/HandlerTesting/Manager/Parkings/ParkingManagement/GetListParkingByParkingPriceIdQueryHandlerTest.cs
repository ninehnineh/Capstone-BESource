using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByParkingPriceId;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Parkings.ParkingManagement
{
    public class GetListParkingByParkingPriceIdQueryHandlerTest
    {
        private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;
        private readonly Mock<IParkingPriceRepository> _parkingPriceRepositoryMock;
        private readonly GetListParkingByParkingPriceIdQueryHandler _handler;
        public GetListParkingByParkingPriceIdQueryHandlerTest()
        {
            _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
            _parkingPriceRepositoryMock = new Mock<IParkingPriceRepository>();
            _handler = new GetListParkingByParkingPriceIdQueryHandler(_parkingHasPriceRepositoryMock.Object, _parkingPriceRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ParkingPriceNotFound_ShouldReturnNotFound()
        {
            // Arrange
            int parkingPriceId = 1; // Replace with a non-existing parkingPriceId


            _parkingPriceRepositoryMock.Setup(repo => repo.GetById(parkingPriceId)).ReturnsAsync((Domain.Entities.ParkingPrice)null);

            var query = new GetListParkingByParkingPriceIdQuery
            {
                ParkingPriceId = parkingPriceId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy gói.");
            result.Data.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_NoParkingHasPriceFound_ShouldReturnNotFound()
        {
            // Arrange
            int parkingPriceId = 1; // Replace with an existing parkingPriceId


            var parkingPrice = new Domain.Entities.ParkingPrice
            {
                ParkingPriceId = parkingPriceId
            };

            _parkingPriceRepositoryMock.Setup(repo => repo.GetById(parkingPriceId)).ReturnsAsync(parkingPrice);
            _parkingHasPriceRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(
                It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
                It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(),
                It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, int>>>(),
                It.IsAny<bool>())).ReturnsAsync(new List<Domain.Entities.ParkingHasPrice>());


            var query = new GetListParkingByParkingPriceIdQuery
            {
                ParkingPriceId = parkingPriceId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy bãi áp dụng gói.");
            result.Data.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_ParkingHasPriceFound_ShouldReturnListOfParking()
        {
            // Arrange
            int parkingPriceId = 1; // Replace with an existing parkingPriceId


            var parkingPrice = new Domain.Entities.ParkingPrice
            {
                ParkingPriceId = parkingPriceId
            };

            var parkings = new List<Domain.Entities.ParkingHasPrice>
            {
                new Domain.Entities.ParkingHasPrice { ParkingHasPriceId = 1, ParkingPriceId = parkingPriceId, Parking = new Domain.Entities.Parking { ParkingId = 1 } },
                new Domain.Entities.ParkingHasPrice { ParkingHasPriceId = 2, ParkingPriceId = parkingPriceId, Parking = new Domain.Entities.Parking { ParkingId = 2 } }
                // Add more ParkingHasPrice objects as needed for testing pagination
            };

            _parkingPriceRepositoryMock.Setup(repo => repo.GetById(parkingPriceId)).ReturnsAsync(parkingPrice);
            _parkingHasPriceRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(
                It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
                It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(),
                It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, int>>>(),
                It.IsAny<bool>())).ReturnsAsync(parkings);


            var query = new GetListParkingByParkingPriceIdQuery
            {
                ParkingPriceId = parkingPriceId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {
            // Arrange
            int parkingPriceId = 1; // Replace with an existing parkingPriceId


            _parkingPriceRepositoryMock.Setup(repo => repo.GetById(parkingPriceId)).Throws(new Exception("Simulated exception"));


            var query = new GetListParkingByParkingPriceIdQuery
            {
                ParkingPriceId = parkingPriceId
            };

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await _handler.Handle(query, CancellationToken.None));
            // You can also check the specific exception message if needed.
        }
    }
}
