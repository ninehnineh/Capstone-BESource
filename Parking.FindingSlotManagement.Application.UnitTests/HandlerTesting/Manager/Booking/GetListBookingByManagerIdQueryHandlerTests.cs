using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetListBookingByManagerId;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Booking
{
    public class GetListBookingByManagerIdQueryHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IBusinessProfileRepository> _businessProfileRepositoryMock;
        private readonly GetListBookingByManagerIdQueryHandler _handler;
        public GetListBookingByManagerIdQueryHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _businessProfileRepositoryMock = new Mock<IBusinessProfileRepository>();
            _handler = new GetListBookingByManagerIdQueryHandler(_bookingRepositoryMock.Object, _businessProfileRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnListOfBookings()
        {
            // Arrange
            var managerId = 1;
            var request = new GetListBookingByManagerIdQuery { ManagerId = managerId, PageNo = 1, PageSize = 10 };

            var businessProfileId = 100;
            var businessProfile = new Domain.Entities.BusinessProfile { BusinessProfileId = businessProfileId, UserId = managerId };
            var bookings = new List<Domain.Entities.Booking>
        {
            new Domain.Entities.Booking { BookingId = 1, BookingDetails = new List<BookingDetails>{ new BookingDetails { TimeSlot = new TimeSlot { Parkingslot = new ParkingSlot { Floor = new Floor { Parking = new Domain.Entities.Parking { BusinessId = businessProfileId} } } } } }, Status = BookingStatus.Success.ToString() },
            new Domain.Entities.Booking { BookingId = 2, BookingDetails = new List<BookingDetails>{ new BookingDetails { TimeSlot = new TimeSlot { Parkingslot = new ParkingSlot { Floor = new Floor { Parking = new Domain.Entities.Parking { BusinessId = businessProfileId} } } } } }, Status = BookingStatus.Check_In.ToString() },
            new Domain.Entities.Booking { BookingId = 3, BookingDetails = new List<BookingDetails>{ new BookingDetails { TimeSlot = new TimeSlot { Parkingslot = new ParkingSlot { Floor = new Floor { Parking = new Domain.Entities.Parking { BusinessId = businessProfileId} } } } } }, Status = BookingStatus.Done.ToString() }
        };


            _bookingRepositoryMock.Setup(repo => repo.GetListBookingByManagerIdMethod(businessProfileId, request.PageNo, request.PageSize))
                .ReturnsAsync(bookings);

            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
                .ReturnsAsync(businessProfile);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<GetListBookingByManagerIdResponse>>(bookings))
                .Returns(bookings.Select(b => new GetListBookingByManagerIdResponse { BookingId = b.BookingId, Status = b.Status }));


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Thành công");
            result.StatusCode.ShouldBe(200);
            result.Data.ShouldNotBeNull();
            result.Data.Count().ShouldBe(bookings.Count);
            result.Count.ShouldBe(bookings.Count);
        }
        [Fact]
        public async Task Handle_ValidRequestNoBookings_ShouldReturnEmptyList()
        {
            // Arrange
            var managerId = 1;
            var request = new GetListBookingByManagerIdQuery { ManagerId = managerId, PageNo = 1, PageSize = 10 };

            var businessProfileId = 100;
            var businessProfile = new Domain.Entities.BusinessProfile { BusinessProfileId = businessProfileId, UserId = managerId };

            _bookingRepositoryMock.Setup(repo => repo.GetListBookingByManagerIdMethod(businessProfileId, request.PageNo, request.PageSize))
                .ReturnsAsync((List<Domain.Entities.Booking>)null);

            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
                .ReturnsAsync(businessProfile);



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy.");
            result.StatusCode.ShouldBe(200);
            result.Count.ShouldBe(0);
        }

        // Test case for invalid managerId
        [Fact]
        public async Task Handle_InvalidManagerId_ShouldReturnError()
        {
            // Arrange
            var managerId = 999;
            var request = new GetListBookingByManagerIdQuery { ManagerId = managerId, PageNo = 1, PageSize = 10 };

            _businessProfileRepositoryMock.Setup(repo => repo.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BusinessProfile, bool>>>(), null, true))
                .ReturnsAsync((Domain.Entities.BusinessProfile)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe("Không tìm thấy tài khoản doanh nghiệp.");
            result.StatusCode.ShouldBe(200);
            result.Count.ShouldBe(0);
        }
    }
}
