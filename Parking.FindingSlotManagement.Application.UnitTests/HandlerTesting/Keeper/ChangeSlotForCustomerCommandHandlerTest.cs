using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.ChangeSlotForCustomer;
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
    public class ChangeSlotForCustomerCommandHandlerTest
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IBookingDetailsRepository> _bookingDetailsRepositoryMock;
        private readonly Mock<ITimeSlotRepository> _timeSlotRepositoryMock;
        private readonly Mock<IConflictRequestRepository> _conflictRequestRepositoryMock;
        private readonly ChangeSlotForCustomerCommandHandler _handler;
        public ChangeSlotForCustomerCommandHandlerTest()
        {
            _bookingDetailsRepositoryMock = new Mock<IBookingDetailsRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _timeSlotRepositoryMock = new Mock<ITimeSlotRepository>();
            _conflictRequestRepositoryMock = new Mock<IConflictRequestRepository>();
            _handler = new ChangeSlotForCustomerCommandHandler(_bookingRepositoryMock.Object, _bookingDetailsRepositoryMock.Object, _timeSlotRepositoryMock.Object, _conflictRequestRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ValidBookingIdAndExistingBookingWithValidTimeSlotChange_ShouldReturnSuccessResponse()
        {
            // Arrange
            int bookingId = 1;
            int parkingSlotId = 100;
            var request = new ChangeSlotForCustomerCommand { BookingId = bookingId, ParkingSlotId = parkingSlotId };

            // Create mock data for the existing booking
            var existingBooking = new Domain.Entities.Booking
            {
                BookingId = bookingId,
                StartTime = DateTime.UtcNow.AddHours(1),
                EndTime = DateTime.UtcNow.AddHours(3),
                BookingDetails = new List<BookingDetails>
            {
                new BookingDetails
                {
                    TimeSlot = new TimeSlot
                    {
                        TimeSlotId = 789,
                        Status = TimeSlotStatus.Booked.ToString()
                    }
                }
            }
            };

            // Create mock data for the new time slot
            var newTimeSlot = new TimeSlot
            {
                TimeSlotId = 101,
                Status = TimeSlotStatus.Free.ToString()
            };

            // Mock the IBookingRepository
            _bookingRepositoryMock.Setup(repo => repo.GetById(bookingId)).ReturnsAsync(existingBooking);

            // Mock the IBookingDetailsRepository
            _bookingDetailsRepositoryMock.Setup(repo => repo.GetParkingSlotIdByBookingDetail(bookingId))
                .ReturnsAsync(existingBooking.BookingDetails);
            _bookingDetailsRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<System.Linq.Expressions.Expression<Func<BookingDetails, bool>>>(), null, null, false))
                .ReturnsAsync(existingBooking.BookingDetails);

            // Mock the ITimeSlotRepository

            _timeSlotRepositoryMock.Setup(repo => repo.GetAllTimeSlotsBooking(existingBooking.StartTime, existingBooking.EndTime.Value, parkingSlotId))
                .ReturnsAsync(new List<TimeSlot> { newTimeSlot });
            var oldTimeSlot = new TimeSlot
            {
                TimeSlotId = 11,
                Status = TimeSlotStatus.Booked.ToString()
            };
            _timeSlotRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<System.Linq.Expressions.Expression<Func<TimeSlot, bool>>>(), null, null, false))
                .ReturnsAsync(new List<TimeSlot> { oldTimeSlot });

            var conflictRequestExist = new ConflictRequest
            {
                ConflictRequestId = 1,
                BookingId = 1,
                Status = ConflictRequestStatus.InProcess.ToString(),

            };
            _conflictRequestRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<System.Linq.Expressions.Expression<Func<ConflictRequest, bool>>>(), null, false)).ReturnsAsync(conflictRequestExist);
            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(204);
            result.Message.ShouldBe("Thành công");
            // You can add more assertions to check if the booking details have been updated correctly.
            // For example, verify that the old booking details have been deleted, and the new booking details have been added.
        }
        [Fact]
        public async Task Handle_InvalidBookingId_ShouldReturnErrorResponse()
        {
            // Arrange
            int bookingId = 1;
            int parkingSlotId = 100;
            var request = new ChangeSlotForCustomerCommand { BookingId = bookingId, ParkingSlotId = parkingSlotId };

            // Mock the IBookingRepository to return null (booking not found)

            _bookingRepositoryMock.Setup(repo => repo.GetById(bookingId)).ReturnsAsync((Domain.Entities.Booking)null);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy đơn.");
        }
        [Fact]
        public async Task Handle_ExistingBookingWithInvalidParkingSlotId_ShouldReturnErrorResponse()
        {
            // Arrange
            int bookingId = 1;
            int parkingSlotId = 100;
            var request = new ChangeSlotForCustomerCommand { BookingId = bookingId, ParkingSlotId = parkingSlotId };

            // Create mock data for the existing booking
            var existingBooking = new Domain.Entities.Booking
            {
                BookingId = bookingId,
                StartTime = DateTime.UtcNow.AddHours(1),
                EndTime = DateTime.UtcNow.AddHours(3),
                BookingDetails = new List<BookingDetails>
            {
                new BookingDetails
                {
                    TimeSlot = new TimeSlot
                    {
                        TimeSlotId = 789,
                        Status = TimeSlotStatus.Booked.ToString()
                    }
                }
            }
            };

            // Create mock data for the new time slot
            var newTimeSlot = new TimeSlot
            {
                TimeSlotId = 101,
                Status = TimeSlotStatus.Free.ToString()
            };

            // Mock the IBookingRepository
            _bookingRepositoryMock.Setup(repo => repo.GetById(bookingId)).ReturnsAsync(existingBooking);

            // Mock the IBookingDetailsRepository
            _bookingDetailsRepositoryMock.Setup(repo => repo.GetParkingSlotIdByBookingDetail(bookingId))
                .ReturnsAsync(existingBooking.BookingDetails);
            _bookingDetailsRepositoryMock.Setup(repo => repo.GetAllItemWithCondition(It.IsAny<System.Linq.Expressions.Expression<Func<BookingDetails, bool>>>(), null, null, false))
                .ReturnsAsync(existingBooking.BookingDetails);

            // Mock the ITimeSlotRepository

            _timeSlotRepositoryMock.Setup(repo => repo.GetAllTimeSlotsBooking(existingBooking.StartTime, existingBooking.EndTime.Value, parkingSlotId))
                .ReturnsAsync(new List<TimeSlot> { newTimeSlot });
            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(404);
            result.Message.ShouldBe("Không tìm thấy time slot cũ.");
        }
    }
}
