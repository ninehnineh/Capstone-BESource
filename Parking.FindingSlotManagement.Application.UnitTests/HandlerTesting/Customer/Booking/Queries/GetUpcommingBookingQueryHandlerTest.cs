using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetUpcommingBooking;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.Booking.Queries
{
    public class GetUpcommingBookingQueryHandlerTest
    {
        private readonly GetUpcommingBookingQueryHandler _handler;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        public GetUpcommingBookingQueryHandlerTest()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetUpcommingBookingQueryHandler(_bookingRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userId = 123;
            var request = new GetUpcommingBookingQuery { UserId = userId };

            _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _bookingRepositoryMock.Verify(x => x.GetUpcommingBookingByUserIdMethod(It.IsAny<int>()), Times.Never);
        }
        [Fact]
        public async Task Handle_NoUpcomingBookings_ReturnsEmptyResponse()
        {
            // Arrange
            var userId = 123;
            var request = new GetUpcommingBookingQuery { UserId = userId };

            var user = new User { UserId = userId };
            _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync(user);
            _bookingRepositoryMock.Setup(x => x.GetUpcommingBookingByUserIdMethod(userId)).ReturnsAsync((IEnumerable<Domain.Entities.Booking>)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy đơn đặt.");
            result.Data.ShouldBeNull();
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _bookingRepositoryMock.Verify(x => x.GetUpcommingBookingByUserIdMethod(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetUpcommingBookingResponse>(It.IsAny<Domain.Entities.Booking>()), Times.Never);
        }
        [Fact]
        public async Task Handle_UpcomingBookingsFound_ReturnsValidResponse()
        {
            // Arrange
            var userId = 123;
            var request = new GetUpcommingBookingQuery { UserId = userId };

            var user = new User { UserId = userId };
            var bookings = new List<Domain.Entities.Booking>
            {
                new Domain.Entities.Booking { BookingId = 1, VehicleInfor = new VehicleInfor{ VehicleInforId = 1}, BookingDetails = new List<BookingDetails> { new BookingDetails { BookingDetailsId = 1, TimeSlotId = 1, TimeSlot = new TimeSlot { TimeSlotId = 1, ParkingSlotId = 1, Parkingslot = new Domain.Entities.ParkingSlot { ParkingSlotId = 1, FloorId = 1, Floor = new Floor { FloorId = 1, ParkingId = 1, Parking = new Domain.Entities.Parking { ParkingId = 1} } } } } } },
            };


            _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync(user);
            _bookingRepositoryMock.Setup(x => x.GetUpcommingBookingByUserIdMethod(userId)).ReturnsAsync(bookings);
            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            result.Data.ShouldNotBeNull();
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _bookingRepositoryMock.Verify(x => x.GetUpcommingBookingByUserIdMethod(It.IsAny<int>()), Times.Once);
        }
    }
}
