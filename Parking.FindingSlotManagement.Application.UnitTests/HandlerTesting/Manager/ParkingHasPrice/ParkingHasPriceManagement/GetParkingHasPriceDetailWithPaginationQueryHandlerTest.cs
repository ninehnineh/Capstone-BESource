using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetParkingHasPriceDetailWithPagination;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.ParkingHasPrice.ParkingHasPriceManagement
{
    public class GetParkingHasPriceDetailWithPaginationQueryHandlerTest
    {
        private readonly Mock<IParkingHasPriceRepository> _parkingHasPriceRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetParkingHasPriceDetailWithPaginationQueryHandler _handler;

        public GetParkingHasPriceDetailWithPaginationQueryHandlerTest()
        {
            _parkingHasPriceRepositoryMock = new Mock<IParkingHasPriceRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetParkingHasPriceDetailWithPaginationQueryHandler(_parkingHasPriceRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_ExistingParkingHasPriceRecord_ShouldReturnResponseWithData()
        {
            // Arrange
            int parkingHasPriceId = 1; // Replace with a valid parkingHasPriceId

            var mockParkingHasPriceRepository = new Mock<IParkingHasPriceRepository>();
            var mockMapper = new Mock<IMapper>();

            var parkingHasPrice = new Domain.Entities.ParkingHasPrice
            {
               ParkingHasPriceId = 1
            };

            mockParkingHasPriceRepository.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
                It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(), true
            )).ReturnsAsync(parkingHasPrice);

            var handler = new GetParkingHasPriceDetailWithPaginationQueryHandler(
                mockParkingHasPriceRepository.Object,
                mockMapper.Object
            );
            var query = new GetParkingHasPriceDetailWithPaginationQuery
            {
                ParkingHasPriceId = parkingHasPriceId
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Count.ShouldBe(1);
            result.Message.ShouldBe("Thành Công");
        }
        [Fact]
        public async Task Handle_NonExistingParkingHasPriceRecord_ShouldReturnEmptyResponse()
        {
            // Arrange
            int parkingHasPriceId = 1; // Replace with a valid parkingHasPriceId

            var mockParkingHasPriceRepository = new Mock<IParkingHasPriceRepository>();
            var mockMapper = new Mock<IMapper>();

            mockParkingHasPriceRepository.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
                It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(), true
            )).ReturnsAsync((Domain.Entities.ParkingHasPrice)null);

            var handler = new GetParkingHasPriceDetailWithPaginationQueryHandler(
                mockParkingHasPriceRepository.Object,
                mockMapper.Object
            );
            var query = new GetParkingHasPriceDetailWithPaginationQuery
            {
                ParkingHasPriceId = parkingHasPriceId
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
            result.Count.ShouldBe(0);
            result.Message.ShouldBe("Không tim thấy");
        }
        [Fact]
        public async Task Handle_ExceptionThrown_ShouldThrowException()
        {
            // Arrange
            int parkingHasPriceId = 1; // Replace with a valid parkingHasPriceId

            var mockParkingHasPriceRepository = new Mock<IParkingHasPriceRepository>();
            var mockMapper = new Mock<IMapper>();

            mockParkingHasPriceRepository.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<Domain.Entities.ParkingHasPrice, bool>>>(),
                It.IsAny<List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>>(), true
            )).Throws(new Exception("Simulated exception"));

            var handler = new GetParkingHasPriceDetailWithPaginationQueryHandler(
                mockParkingHasPriceRepository.Object,
                mockMapper.Object
            );
            var query = new GetParkingHasPriceDetailWithPaginationQuery
            {
                ParkingHasPriceId = parkingHasPriceId
            };

            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await handler.Handle(query, CancellationToken.None));
            // You can also check the specific exception message if needed.
        }
    }
}
