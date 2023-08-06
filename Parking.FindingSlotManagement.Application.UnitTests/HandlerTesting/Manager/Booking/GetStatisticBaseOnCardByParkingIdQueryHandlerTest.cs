using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetStatisticBaseOnCardByParkingId;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Booking
{
    public class GetStatisticBaseOnCardByParkingIdQueryHandlerTest
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetStatisticBaseOnCardByParkingIdQueryHandler _handler;
        public GetStatisticBaseOnCardByParkingIdQueryHandlerTest()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new GetStatisticBaseOnCardByParkingIdQueryHandler(_bookingRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidParkingId_ShouldReturnStatistics()
        {
            // Arrange
            var parkingId = 301;
            var request = new GetStatisticBaseOnCardByParkingIdQuery { ParkingId = parkingId };

            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId };

            _parkingRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, true))
                .ReturnsAsync(parkingExist);


            _bookingRepositoryMock
                .Setup(repo => repo.GetTotalOrdersByParkingIdMethod(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(id * 100)); // For simplicity, assuming the number of orders is parkingId * 100

            _bookingRepositoryMock
                .Setup(repo => repo.GetRevenueByParkingIdMethod(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult((decimal)(id * 1000))); // For simplicity, assuming the revenue is parkingId * 1000

            _bookingRepositoryMock
                .Setup(repo => repo.GetTotalNumberOfOrdersInCurrentDayByParkingIdMethod(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(id * 10)); // For simplicity, assuming the total number of orders in current day is parkingId * 10

            _bookingRepositoryMock
                .Setup(repo => repo.GetTotalWaitingOrdersByParkingIdMethod(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(id)); // For simplicity, assuming the total waiting orders is equal to parkingId



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            result.Data.NumberOfOrders.ShouldBe(30100); // 301 * 100 = 30100
            result.Data.TotalOfRevenue.ShouldBe(301000); // 301 * 1000 = 301000
            result.Data.NumberOfOrdersInCurrentDay.ShouldBe(3010); // 301 * 10 = 3010
            result.Data.WaitingOrder.ShouldBe(301); // 301
        }
        [Fact]
        public async Task Handle_InvalidParkingId_ShouldReturnError()
        {
            // Arrange
            var parkingId = 999;
            var request = new GetStatisticBaseOnCardByParkingIdQuery { ParkingId = parkingId };


            _parkingRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, true))
                .ReturnsAsync((Domain.Entities.Parking)null);



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy thông tin bãi giữ xe.");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
        }
    }
}
