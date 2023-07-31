using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetCustomerActivities;
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
    public class GetCustomerActivitiesQueryHandlerTest
    {
        private readonly GetCustomerActivitiesQueryHandler _handler;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        public GetCustomerActivitiesQueryHandlerTest()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetCustomerActivitiesQueryHandler(_bookingRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userId = 123;
            var request = new GetCustomerActivitiesQuery { UserId = userId };

            _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync((Domain.Entities.User)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy tài khoản.");
            result.Data.ShouldBeNull();
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _bookingRepositoryMock.Verify(x => x.GetCustomerActivitiesByUserIdMethod(It.IsAny<int>()), Times.Never);
        }
        [Fact]
        public async Task Handle_BookingsNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userId = 123;
            var request = new GetCustomerActivitiesQuery { UserId = userId };

            var user = new User { UserId = userId };
            _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync(user);
            _bookingRepositoryMock.Setup(x => x.GetCustomerActivitiesByUserIdMethod(userId)).ReturnsAsync((IEnumerable<Domain.Entities.Booking>)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy đơn đặt.");
            result.Data.ShouldBeNull();
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _bookingRepositoryMock.Verify(x => x.GetCustomerActivitiesByUserIdMethod(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(x => x.Map<BookingSearchResult>(It.IsAny<Domain.Entities.Booking>()), Times.Never);

        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var userId = 123;
            var request = new GetCustomerActivitiesQuery { UserId = userId };

            var user = new User { UserId = userId };
            var bookings = new List<Domain.Entities.Booking>
            {
                new Domain.Entities.Booking
                {
                    BookingDetails = new List<BookingDetails>
                    {
                        new BookingDetails
                        {
                            TimeSlot = new TimeSlot
                            {
                                Parkingslot = new Domain.Entities.ParkingSlot
                                {
                                    Floor = new Floor { Parking = new Domain.Entities.Parking() }
                                }
                            }
                        }
                    }
                }
            };



            _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync(user);
            _bookingRepositoryMock.Setup(x => x.GetCustomerActivitiesByUserIdMethod(userId)).ReturnsAsync(bookings);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _bookingRepositoryMock.Verify(x => x.GetCustomerActivitiesByUserIdMethod(It.IsAny<int>()), Times.Once);
            _mapperMock.Verify(x => x.Map<BookingSearchResult>(It.IsAny<Domain.Entities.Booking>()), Times.Once);
            // Continue verifying other map calls as necessary
        }
    }
}
