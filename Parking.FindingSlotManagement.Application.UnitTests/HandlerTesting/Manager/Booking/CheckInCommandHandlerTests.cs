using Microsoft.Extensions.Configuration;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckIn;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
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
    public class CheckInCommandHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IFireBaseMessageServices> _fireBaseMessageServicesMock;
        private readonly Mock<ITimeSlotRepository> _timeSlotRepositoryMock;
        private readonly Mock<IBookingDetailsRepository> _bookingDetailsRepositoryMock;
        private readonly Mock<IConflictRequestRepository> _conflictRequestRepositoryMock;
        private readonly CheckInCommandHandler _handler;
        public CheckInCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _fireBaseMessageServicesMock = new Mock<IFireBaseMessageServices>();
            _timeSlotRepositoryMock = new Mock<ITimeSlotRepository>();
            _bookingDetailsRepositoryMock = new Mock<IBookingDetailsRepository>();
            _conflictRequestRepositoryMock = new Mock<IConflictRequestRepository>();
            _handler = new CheckInCommandHandler(_bookingRepositoryMock.Object, _configurationMock.Object, _fireBaseMessageServicesMock.Object, _timeSlotRepositoryMock.Object, _bookingDetailsRepositoryMock.Object, _conflictRequestRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_NonExistingBooking_ShouldReturnNotFound()
        {
            // Arrange
            var bookingId = 1; // Replace with the non-existing booking ID for testing
            var checkInCommand = new CheckInCommand { BookingId = bookingId };

            _bookingRepositoryMock.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<Domain.Entities.Booking, bool>>>(),
                It.IsAny<List<Expression<Func<Domain.Entities.Booking, object>>>>(),
                It.IsAny<bool>())).ReturnsAsync((Domain.Entities.Booking)null);


            // Act
            var result = await _handler.Handle(checkInCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Đơn đặt không tồn tại.");
        }
        [Fact]
        public async Task Handle_BookingInDifferentStatus_ShouldReturnError()
        {

            var bookingId = 1; // Replace with the booking ID for testing
            var checkInCommand = new CheckInCommand { BookingId = bookingId };

            var existingBooking = new Domain.Entities.Booking
            {
                BookingId = bookingId,
                Status = BookingStatus.Cancel.ToString(),
                /* Other properties */
            };

            _bookingRepositoryMock.Setup(repo => repo.GetBookingDetailsByBookingIdMethod(bookingId)).ReturnsAsync(existingBooking);



            // Act
            var result = await _handler.Handle(checkInCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Đơn đang ở trạng thái khác hoặc đã bị hủy nên không thể xử lý.");
        }
        [Fact]
        public async Task Handle_BookingWithConflictRequest_Bao_Tri_ShouldReturnError()
        {


            var bookingId = 1; // Replace with the booking ID for testing
            var checkInCommand = new CheckInCommand { BookingId = bookingId };

            var existingBooking = new Domain.Entities.Booking
            {
                BookingId = bookingId,
                Status = BookingStatus.Success.ToString(),
                BookingDetails = new List<BookingDetails>
                { 
                    new BookingDetails {BookingDetailsId = 1, BookingId = 1, TimeSlot = new TimeSlot{ TimeSlotId = 1, Status = TimeSlotStatus.Busy.ToString()} },
                    new BookingDetails {BookingDetailsId = 2, BookingId = 1, TimeSlot = new TimeSlot{ TimeSlotId = 2, Status = TimeSlotStatus.Busy.ToString()} },
                }
                /* Other properties */
            };

            var existingConflictRequest = new ConflictRequest
            {
                BookingId = bookingId,
                Message = ConflictRequestMessage.Bao_tri.ToString(),
                Status = ConflictRequestStatus.InProcess.ToString(),
                /* Other properties */
            };

            _bookingRepositoryMock.Setup(repo => repo.GetBookingDetailsByBookingIdMethod(bookingId)).ReturnsAsync(existingBooking);

            _conflictRequestRepositoryMock.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<ConflictRequest, bool>>>(),null, true)).ReturnsAsync(existingConflictRequest);



            // Act
            var result = await _handler.Handle(checkInCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Đơn đặt xảy ra lỗi. Do slot đang bảo trì.");
        }
        [Fact]
        public async Task Handle_BookingWithConflictRequest_Qua_gio_ShouldReturnError()
        {


            var bookingId = 1; // Replace with the booking ID for testing
            var checkInCommand = new CheckInCommand { BookingId = bookingId };

            var existingBooking = new Domain.Entities.Booking
            {
                BookingId = bookingId,
                Status = BookingStatus.Success.ToString(),
                BookingDetails = new List<BookingDetails>
                {
                    new BookingDetails {BookingDetailsId = 1, BookingId = 1, TimeSlot = new TimeSlot{ TimeSlotId = 1, Status = TimeSlotStatus.Booked.ToString()} },
                    new BookingDetails {BookingDetailsId = 2, BookingId = 1, TimeSlot = new TimeSlot{ TimeSlotId = 2, Status = TimeSlotStatus.Booked.ToString()} },
                }
                /* Other properties */
            };

            var existingConflictRequest = new ConflictRequest
            {
                BookingId = bookingId,
                Message = ConflictRequestMessage.Qua_gio.ToString(),
                Status = ConflictRequestStatus.InProcess.ToString(),
                /* Other properties */
            };

            _bookingRepositoryMock.Setup(repo => repo.GetBookingDetailsByBookingIdMethod(bookingId)).ReturnsAsync(existingBooking);

            _conflictRequestRepositoryMock.Setup(repo => repo.GetItemWithCondition(
                It.IsAny<Expression<Func<ConflictRequest, bool>>>(), null, true)).ReturnsAsync(existingConflictRequest);



            // Act
            var result = await _handler.Handle(checkInCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Đơn đặt xảy ra lỗi. Do slot đặt có đơn khác lấn giờ.");
        }
        [Fact]
        public async Task Handle_BookingWithNonSuccessStatus_ShouldReturnError()
        {
            // Arrange
            var bookingId = 1;
            var request = new CheckInCommand { BookingId = bookingId };

            var booking = new Domain.Entities.Booking
            {
                BookingId = bookingId,
                Status = BookingStatus.Check_Out.ToString(), // Set status to a non-Success status
                StartTime = DateTime.UtcNow.AddHours(1),
                EndTime = DateTime.UtcNow.AddHours(2),
                User = new User
                {
                    Devicetoken = "user_device_token"
                }
            };

            _bookingRepositoryMock.Setup(repo => repo.GetBookingDetailsByBookingIdMethod(bookingId)).ReturnsAsync(booking);



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Đơn đang ở trạng thái khác hoặc đã bị hủy nên không thể xử lý.");
            result.StatusCode.ShouldBe(400);
        }
    }
}
