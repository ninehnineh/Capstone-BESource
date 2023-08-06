using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetBookingById;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Booking
{
    public class GetBookingByIdQueryHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly GetBookingByIdQueryHandler _handler;
        public GetBookingByIdQueryHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _handler = new GetBookingByIdQueryHandler(_bookingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_NonExistentBooking_ShouldReturnError()
        {
            // Arrange
            var bookingId = 1;
            var request = new GetBookingByIdQuery { BookingId = bookingId };

            _bookingRepositoryMock.Setup(repo => repo.GetListBookingByBookingIdMethod(bookingId))
                .ReturnsAsync((Domain.Entities.Booking)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy.");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_ExistingBooking_ShouldReturnSuccess()
        {
            // Arrange
            var bookingId = 1;
            var request = new GetBookingByIdQuery { BookingId = bookingId };

            var booking = new Domain.Entities.Booking
            {
                BookingId = bookingId,
                User = new User { UserId = 100, Name = "John Doe" },
                VehicleInfor = new VehicleInfor { VehicleInforId = 200, LicensePlate = "ABC-123" }
                // Add other properties as needed
            };

            _bookingRepositoryMock.Setup(repo => repo.GetListBookingByBookingIdMethod(bookingId))
                .ReturnsAsync(booking);

            

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            result.Data.BookingId.ShouldBe(bookingId);
            // Add assertions for other properties of GetBookingByIdResponse as needed
        }
    }
}
