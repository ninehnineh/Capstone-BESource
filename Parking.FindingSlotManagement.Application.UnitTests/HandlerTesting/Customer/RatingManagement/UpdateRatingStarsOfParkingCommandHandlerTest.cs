using FluentValidation.TestHelper;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.RatingParking.RatingManagement.Commands.UpdateRatingStarsOfParking;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.RatingManagement
{
    public class UpdateRatingStarsOfParkingCommandHandlerTest
    {
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly UpdateRatingStarsOfParkingCommandHandler _handler;
        private readonly UpdateRatingStarsOfParkingCommandValidation _validator;
        public UpdateRatingStarsOfParkingCommandHandlerTest()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _validator = new UpdateRatingStarsOfParkingCommandValidation();
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _handler = new UpdateRatingStarsOfParkingCommandHandler(_parkingRepositoryMock.Object, _bookingRepositoryMock.Object);
        }
        [Fact]
        public async Task UpdateRatingStarsOfParkingCommandHandler_Should_Update_Parking_Successfully()
        {
            // Arrange
            var request = new UpdateRatingStarsOfParkingCommand
            {
                ParkingId = 1,
                Stars = 5,
                BookingId = 1
            };
            var cancellationToken = new CancellationToken();
            var OldStarsOfParking = new Domain.Entities.Parking
            {
                ParkingId = 1,
                Stars = null,
                TotalStars = null,
                StarsCount = null
            };
            var expectedBooking = new Domain.Entities.Booking
            {
                BookingId = 1,
            };
            _bookingRepositoryMock.Setup(x => x.GetById(request.BookingId)).ReturnsAsync(expectedBooking);
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync(OldStarsOfParking);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Thành công");
            response.StatusCode.ShouldBe(204);
            response.Count.ShouldBe(0);
            OldStarsOfParking.Stars.ShouldBe(request.Stars);
            expectedBooking.IsRating.ShouldBe(true);
            _parkingRepositoryMock.Verify(x => x.Save(), Times.Once);
            _bookingRepositoryMock.Verify(x => x.Save(), Times.Once);
        }
        [Fact]
        public async Task UpdateRatingStarsOfParkingCommandHandler_Should_Return_Not_Found_If_Parking_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateRatingStarsOfParkingCommand
            {
                ParkingId = 100000,
                Stars = 5,
                BookingId = 1
            };
            var cancellationToken = new CancellationToken();
            var expectedBooking = new Domain.Entities.Booking
            {
                BookingId = 1,
            };
            _bookingRepositoryMock.Setup(x => x.GetById(request.BookingId)).ReturnsAsync(expectedBooking);
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeTrue();
            response.Message.ShouldBe("Không tìm thấy bãi giữ xe.");
            response.StatusCode.ShouldBe(200);
            response.Count.ShouldBe(0);
            _parkingRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public async Task UpdateRatingStarsOfParkingCommandHandler_Should_Return_Not_Found_If_Booking_Does_Not_Exist()
        {
            // Arrange
            var request = new UpdateRatingStarsOfParkingCommand
            {
                ParkingId = 100000,
                Stars = 5,
                BookingId = 1
            };
            var cancellationToken = new CancellationToken();

            _bookingRepositoryMock.Setup(x => x.GetById(request.BookingId)).ReturnsAsync((Domain.Entities.Booking)null);
            _parkingRepositoryMock.Setup(x => x.GetById(request.ParkingId))
                .ReturnsAsync((Domain.Entities.Parking)null);

            // Act
            var response = await _handler.Handle(request, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();
            response.Message.ShouldBe("Không tìm thấy đơn đặt.");
            response.StatusCode.ShouldBe(404);
            response.Count.ShouldBe(0);
            _parkingRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
        [Fact]
        public void ParkingId_ShouldNotBeEmpty()
        {
            var command = new UpdateRatingStarsOfParkingCommand
            {
                Stars = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
        }
        [Fact]
        public void ParkingId_ShouldNotBeNull()
        {
            var command = new UpdateRatingStarsOfParkingCommand
            {
                Stars = 5
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ParkingId);
        }

        [Fact]
        public void Stars_ShouldNotBeEmpty()
        {
            var command = new UpdateRatingStarsOfParkingCommand
            {
                ParkingId = 1,
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Stars);
        }
        [Fact]
        public void Stars_ShouldNotBeNull()
        {
            var command = new UpdateRatingStarsOfParkingCommand
            {
                ParkingId = 1,
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Stars);
        }
        [Fact]
        public void Stars_ShouldNotGreaterThan_5()
        {
            var command = new UpdateRatingStarsOfParkingCommand
            {
                ParkingId = 1,
                Stars = 6
            };
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Stars);
        }
        [Fact]
        public void Stars_ShouldNotLessThan_1()
        {
            var command = new UpdateRatingStarsOfParkingCommand
            {
                ParkingId = 1,
                Stars = 0
            };
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Stars);
        }
    }
}
