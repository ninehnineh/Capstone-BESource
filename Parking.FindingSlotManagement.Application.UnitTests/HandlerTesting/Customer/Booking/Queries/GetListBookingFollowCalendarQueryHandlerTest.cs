using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetListBookingFollowCalendar;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.Booking.Queries
{
    public class GetListBookingFollowCalendarQueryHandlerTest
    {
        private readonly GetListBookingFollowCalendarQueryHandler _handler;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        public GetListBookingFollowCalendarQueryHandlerTest()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _handler = new GetListBookingFollowCalendarQueryHandler(_bookingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_NoBookings_ReturnsEmptyResponse()
        {
            // Arrange
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(7);
            var request = new GetListBookingFollowCalendarQuery { Start = startDate, End = endDate };

            _bookingRepositoryMock.Setup(x => x.GetListBookingFollowCalendarMethod(startDate, endDate)).ReturnsAsync(new List<Domain.Entities.Booking>());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Không tìm thấy.");
            _bookingRepositoryMock.Verify(x => x.GetListBookingFollowCalendarMethod(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }
        [Fact]
        public async Task Handle_BookingsFound_ReturnsValidResponse()
        {
            // Arrange
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(7);
            var request = new GetListBookingFollowCalendarQuery { Start = startDate, End = endDate };

            var bookings = new List<Domain.Entities.Booking>
            {
                new Domain.Entities.Booking { BookingId = 1 },
                new Domain.Entities.Booking { BookingId = 2 }
            };



            _bookingRepositoryMock.Setup(x => x.GetListBookingFollowCalendarMethod(startDate, endDate)).ReturnsAsync(bookings);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            result.Count.ShouldBe(2);
            _bookingRepositoryMock.Verify(x => x.GetListBookingFollowCalendarMethod(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            
        }
    }
}
