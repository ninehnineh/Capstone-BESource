using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetNumberOfDoneAndCancelBookingByParkingId;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Booking
{
    public class GetNumberOfDoneAndCancelBookingByParkingIdHandlerTest
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly GetNumberOfDoneAndCancelBookingByParkingIdHandler _handler;
        public GetNumberOfDoneAndCancelBookingByParkingIdHandlerTest()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new GetNumberOfDoneAndCancelBookingByParkingIdHandler(_bookingRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidRequestWithBookings_ShouldReturnNumberOfDoneAndCancelBookings()
        {
            // Arrange
            var parkingId = 101;
            var request = new GetNumberOfDoneAndCancelBookingByParkingIdQuery { ParkingId = parkingId };

            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId };


            _parkingRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, true))
                .ReturnsAsync(parkingExist);


            _bookingRepositoryMock
                .Setup(repo => repo.GetListBookingDoneOrCancelByParkingIdMethod(
                    It.IsAny<int>(), It.IsAny<string>()))
                .Returns<int, string>((id, status) =>
                {
                    // Assuming the parkingId is the same as the request parkingId
                    if (id == parkingId)
                    {
                        return Task.FromResult(status == Domain.Enum.BookingStatus.Done.ToString() ? 3 : 2);
                    }
                    return Task.FromResult(0);
                });


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            result.Data.NumberOfDoneBooking.ShouldBe(3); // 3 done bookings for parkingId 101
            result.Data.NumberOfCancelBooking.ShouldBe(2); // 2 cancel bookings for parkingId 101
            result.Data.Total.ShouldBe(5); // Total done + cancel bookings
        }
        // Test case for valid request with no bookings
        [Fact]
        public async Task Handle_ValidRequestNoBookings_ShouldReturnZero()
        {
            // Arrange
            var parkingId = 101;
            var request = new GetNumberOfDoneAndCancelBookingByParkingIdQuery { ParkingId = parkingId };

            var parkingExist = new Domain.Entities.Parking { ParkingId = parkingId };

            
            _parkingRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.Parking, bool>>>(), null, true))
                .ReturnsAsync(parkingExist);

            _bookingRepositoryMock
                .Setup(repo => repo.GetListBookingDoneOrCancelByParkingIdMethod(
                    It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(0);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            result.Data.NumberOfDoneBooking.ShouldBe(0); // No done bookings
            result.Data.NumberOfCancelBooking.ShouldBe(0); // No cancel bookings
            result.Data.Total.ShouldBe(0); // Total done + cancel bookings
        }
        [Fact]
        public async Task Handle_InvalidParkingId_ShouldReturnError()
        {
            // Arrange
            var parkingId = 999;
            var request = new GetNumberOfDoneAndCancelBookingByParkingIdQuery { ParkingId = parkingId };

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
