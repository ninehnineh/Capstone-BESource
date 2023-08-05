using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetBookingDetails;
using Parking.FindingSlotManagement.Application.Models.User;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.Booking.Queries
{
    public class GetBookingDetailsQueryHandlerTest
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetBookingDetailsQueryHandler _handler;
        public GetBookingDetailsQueryHandlerTest()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetBookingDetailsQueryHandler(_bookingRepositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task Handle_BookingFound_ReturnsValidResponse()
        {
            // Arrange
            var bookingId = 123;
            var request = new GetBookingDetailsQuery { BookingId = bookingId };

            var booking = new Domain.Entities.Booking
            {
                BookingId = bookingId,
                BookingDetails = new List<BookingDetails>() { new BookingDetails { BookingId = 1, TimeSlotId = 1, BookingDetailsId = 1, TimeSlot = new TimeSlot() { TimeSlotId = 1, ParkingSlotId = 1, Status = TimeSlotStatus.Booked.ToString(), Parkingslot = new Domain.Entities.ParkingSlot { ParkingSlotId = 1, Floor = new Floor { FloorId = 1, Parking = new Domain.Entities.Parking { ParkingId = 1 } } } } } },
                VehicleInfor = new Domain.Entities.VehicleInfor { VehicleInforId = 1 },
                User = new User { UserId = 1 }
            };
            var bookingDetails = new BookingDetails { TimeSlot = new TimeSlot { Parkingslot = new Domain.Entities.ParkingSlot { Floor = new Floor { Parking = new Domain.Entities.Parking() } } } };

            var expectedResponse = new GetBookingDetailsResponse
            {
                BookingDetails = new BookingDetailsDto(),
                User = new UserBookingDto(),
                VehicleInfor = new VehicleInforDtoos(),
                ParkingSlotWithBookingDetailDto = new ParkingSlotWithBookingDetailDto(),
                FloorWithBookingDetailDto = new FloorWithBookingDetailDto(),
                ParkingWithBookingDetailDto = new ParkingWithBookingDetailDto(),
                TransactionWithBookingDetailDtos = new List<TransactionWithBookingDetailDto>()
            };

            _bookingRepositoryMock.Setup(x => x.GetBookingDetailsByBookingIdMethod(bookingId)).ReturnsAsync(booking);
            _bookingRepositoryMock.Setup(x => x.GetBookingDetailsByBookingIdMethod(999)).ReturnsAsync((Domain.Entities.Booking)null);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Thành công");
            _bookingRepositoryMock.Verify(x => x.GetBookingDetailsByBookingIdMethod(It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public async Task Handle_BookingNotFound_ReturnsErrorResponse()
        {
            var bookingId = 123;
            var request = new GetBookingDetailsQuery { BookingId = bookingId };

            var booking = new Domain.Entities.Booking
            {
                BookingId = bookingId,
                BookingDetails = new List<BookingDetails>() { new BookingDetails { BookingId = 1, TimeSlotId = 1, BookingDetailsId = 1, TimeSlot = new TimeSlot() { TimeSlotId = 1, ParkingSlotId = 1, Status = TimeSlotStatus.Booked.ToString(), Parkingslot = new Domain.Entities.ParkingSlot { ParkingSlotId = 1, Floor = new Floor { FloorId = 1, Parking = new Domain.Entities.Parking { ParkingId = 1 } } } } } },
                VehicleInfor = new Domain.Entities.VehicleInfor { VehicleInforId = 1 },
                User = new User { UserId = 1 }
            };
            var bookingDetails = new BookingDetails { TimeSlot = new TimeSlot { Parkingslot = new Domain.Entities.ParkingSlot { Floor = new Floor { Parking = new Domain.Entities.Parking() } } } };


            _bookingRepositoryMock.Setup(x => x.GetBookingDetailsByBookingIdMethod(bookingId)).ReturnsAsync((Domain.Entities.Booking)null);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Đơn đặt không tồn tại");
            result.Data.ShouldBeNull();
            _bookingRepositoryMock.Verify(x => x.GetBookingDetailsByBookingIdMethod(It.IsAny<int>()), Times.Once);
        }

    }
}
