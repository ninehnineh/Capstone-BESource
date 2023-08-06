using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetRevenueByParkingId;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Booking
{
    public class GetRevenueByParkingIdQueryHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetRevenueByParkingIdQueryHandler _handler;
        public GetRevenueByParkingIdQueryHandlerTests()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _handler = new GetRevenueByParkingIdQueryHandler(_bookingRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidRequestWithWeek_ShouldReturnRevenueForWeek()
        {
            // Arrange
            var parkingId = 101;
            var request = new GetRevenueByParkingIdQuery { ParkingId = parkingId, Week = "current" };

            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId };

            
            _parkingRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, true))
                .ReturnsAsync(parkingExist);

            var startDate = new DateTime(2023, 7, 24);
            var endDate = new DateTime(2023, 7, 30);

            _bookingRepositoryMock
                .Setup(repo => repo.GetRevenueByDateByParkingIdMethod(
                    It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns<int, DateTime>((id, date) =>
                {
                    // Assuming the parkingId is the same as the request parkingId
                    if (id == parkingId)
                    {
                        // For simplicity, returning some dummy revenue values for each date
                        if (date == startDate)
                        {
                            return Task.FromResult(100M);
                        }
                        if (date == startDate.AddDays(1))
                        {
                            return Task.FromResult(150M);
                        }
                        // Add similar conditions for other dates in the week
                    }
                    return Task.FromResult(0M);
                });


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            // Add more assertions based on the expected revenue values for each date in the week
            // For example, result.Data should contain a list of 7 GetRevenueByParkingIdResponse objects with the expected revenue values for each date.
        }
        [Fact]
        public async Task Handle_ValidRequestWithMonth_ShouldReturnRevenueForMonth()
        {
            // Arrange
            var parkingId = 101;
            var request = new GetRevenueByParkingIdQuery { ParkingId = parkingId, Month = "current" };

            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId };

            _parkingRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, true))
                .ReturnsAsync(parkingExist);

            var currentDate = new DateTime(2023, 7, 31);
            var daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

            _bookingRepositoryMock
                .Setup(repo => repo.GetRevenueByDateByParkingIdMethod(
                    It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns<int, DateTime>((id, date) =>
                {
                    // Assuming the parkingId is the same as the request parkingId
                    if (id == parkingId)
                    {
                        // For simplicity, returning some dummy revenue values for each date
                        if (date == new DateTime(2023, 7, 1))
                        {
                            return Task.FromResult(200M);
                        }
                        if (date == new DateTime(2023, 7, 2))
                        {
                            return Task.FromResult(300M);
                        }
                        // Add similar conditions for other dates in the month
                    }
                    return Task.FromResult(0M);
                });


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            // Add more assertions based on the expected revenue values for each date in the month
            // For example, result.Data should contain a list of 'daysInMonth' GetRevenueByParkingIdResponse objects with the expected revenue values for each date.
        }
        [Fact]
        public async Task Handle_InvalidParkingId_ShouldReturnError()
        {
            // Arrange
            var parkingId = 999;
            var request = new GetRevenueByParkingIdQuery { ParkingId = parkingId };


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
