using AutoMapper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Admin.Bill.BillManagement.Queries.GetAllBills;
using Parking.FindingSlotManagement.Application.Features.Admin.Booking.BookingManagement.Queries.GetAllBookingForAdmin;
using Parking.FindingSlotManagement.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Admin.Booking
{
    public class GetAllBookingForAdminQueryHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IMapper> _mapper;
        private readonly GetAllBookingForAdminQueryHandler _handler;
        public GetAllBookingForAdminQueryHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _mapper = new Mock<IMapper>();
            _handler = new GetAllBookingForAdminQueryHandler(_bookingRepositoryMock.Object, _mapper.Object);
        }
        [Fact]
        public async Task Handle_WhenRepositoryReturnsNull_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new GetAllBookingForAdminQuery { PageNo = 1, PageSize = 10 };
            _bookingRepositoryMock.Setup(x => x.GetAllBookingForAdminMethod(1, 10)).ReturnsAsync((IEnumerable<Domain.Entities.Booking>)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.StatusCode.ShouldBe(404);
            response.Message.ShouldBe("Không tìm thấy.");
        }
        [Fact]
        public async Task Handle_WhenRepositoryReturnsData_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new GetAllBookingForAdminQuery { PageNo = 1, PageSize = 10 };
            var bookings = new List<Domain.Entities.Booking>
            {
                // Add some sample booking objects here
            };
            var responseDtoList = bookings.Select(b =>
            {
                return new GetAllBookingForAdminResponse
                {
                    BookingDtoForAdmin = _mapper.Object.Map<BookingDtoForAdmin>(b),
                    ParkingDtoForAdmin = _mapper.Object.Map<ParkingDtoForAdmin>(b.BookingDetails.FirstOrDefault()?.TimeSlot.Parkingslot.Floor.Parking),
                    FloorDtoForAdmin = _mapper.Object.Map<FloorDtoForAdmin>(b.BookingDetails.FirstOrDefault()?.TimeSlot.Parkingslot.Floor),
                    SlotDtoForAdmin = _mapper.Object.Map<SlotDtoForAdmin>(b.BookingDetails.FirstOrDefault()?.TimeSlot.Parkingslot)
                };
            }).ToList();

            _bookingRepositoryMock.Setup(x => x.GetAllBookingForAdminMethod(1, 10)).ReturnsAsync(bookings);
            _mapper.Setup(x => x.Map<IEnumerable<GetAllBookingForAdminResponse>>(bookings)).Returns(responseDtoList);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Thành công");
            response.Data.ShouldNotBeNull();
        }
    }
}
