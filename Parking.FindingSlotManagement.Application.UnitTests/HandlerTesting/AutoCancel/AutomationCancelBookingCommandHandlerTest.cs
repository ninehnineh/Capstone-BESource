using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.AutoCancel.commands.AutomationCancelBooking;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.AutoCancel
{
    public class AutomationCancelBookingCommandHandlerTest
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<ITimeSlotRepository> _timeSlotRepositoryMock;
        private readonly AutomationCancelBookingCommandHandler _handler;
        public AutomationCancelBookingCommandHandlerTest()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _timeSlotRepositoryMock = new Mock<ITimeSlotRepository>();
            _handler = new AutomationCancelBookingCommandHandler(_bookingRepositoryMock.Object, _timeSlotRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_WhenBookingDoesNotExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new AutomationCancelBookingCommand { BookingId = 1 };
            _bookingRepositoryMock.Setup(repo => repo.GetBookingIncludeTimeSlot(1))
                                  .ReturnsAsync((Domain.Entities.Booking)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Đơn không tồn tại");
        }
        [Fact]
        public async Task Handle_WhenBookingExistsWithNullCheckinTime_CancelsBookingAndFreesTimeSlots()
        {
            // Arrange
            var request = new AutomationCancelBookingCommand { BookingId = 1 };
            var booking = new Domain.Entities.Booking
            {
                BookingId = 1,
                CheckinTime = null,
                Status = BookingStatus.Success.ToString(),
                BookingDetails = new List<Domain.Entities.BookingDetails>
                {
                    new Domain.Entities.BookingDetails { TimeSlot = new TimeSlot { Status = TimeSlotStatus.Booked.ToString() } },
                new Domain.Entities.BookingDetails { TimeSlot = new TimeSlot { Status = TimeSlotStatus.Booked.ToString() } }
            }
            };
            _bookingRepositoryMock.Setup(repo => repo.GetBookingIncludeTimeSlot(1))
                                  .ReturnsAsync(booking);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.StatusCode.ShouldBe(200);
            response.Message.ShouldBe("Thành công");

            // Check that the booking was canceled and time slots were freed
            booking.Status.ShouldBe(BookingStatus.Cancel.ToString());
            foreach (var item in booking.BookingDetails)
            {
                item.TimeSlot.Status.ShouldBe(TimeSlotStatus.Free.ToString());
            }

            // Check that Save method was called for both repositories
            _bookingRepositoryMock.Verify(repo => repo.Save(), Times.Once);
            _timeSlotRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }
    }
}
