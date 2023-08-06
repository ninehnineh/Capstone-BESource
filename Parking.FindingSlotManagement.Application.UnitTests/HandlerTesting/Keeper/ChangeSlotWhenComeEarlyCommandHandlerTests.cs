using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.ChangeSlotWhenComeEarly;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Keeper
{
    public class ChangeSlotWhenComeEarlyCommandHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IBookingDetailsRepository> _bookingDetailsRepositoryMock;
        private readonly Mock<ITimeSlotRepository> _timeSlotRepositoryMock;
        private readonly ChangeSlotWhenComeEarlyCommandHandler _handler;
        public ChangeSlotWhenComeEarlyCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _bookingDetailsRepositoryMock = new Mock<IBookingDetailsRepository>();
            _timeSlotRepositoryMock = new Mock<ITimeSlotRepository>();
            _handler = new ChangeSlotWhenComeEarlyCommandHandler(_bookingRepositoryMock.Object, _bookingDetailsRepositoryMock.Object, _timeSlotRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ExistingBookingWithInvalidBookingId_ShouldReturnErrorResponse()
        {
            // Arrange
            int bookingId = 1;
            var request = new ChangeSlotWhenComeEarlyCommand { BookingId = bookingId };

            _bookingRepositoryMock.Setup(repo => repo.GetById(bookingId)).ReturnsAsync((Domain.Entities.Booking)null);



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy đơn.");
        }
    }
}
