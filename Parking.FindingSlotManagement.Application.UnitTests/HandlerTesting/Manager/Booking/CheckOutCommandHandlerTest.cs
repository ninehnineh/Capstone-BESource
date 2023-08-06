using Microsoft.Extensions.Configuration;
using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckOut;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Manager.Booking
{
    public class CheckOutCommandHandlerTest
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IFireBaseMessageServices> _fireBaseMessageServicesMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<IParkingRepository> _parkingRepositoryMock;
        private readonly CheckOutCommandHandler _handler;
        public CheckOutCommandHandlerTest()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _fireBaseMessageServicesMock = new Mock<IFireBaseMessageServices>();
            _configurationMock = new Mock<IConfiguration>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _handler = new CheckOutCommandHandler(_bookingRepositoryMock.Object, _fireBaseMessageServicesMock.Object, _configurationMock.Object, _transactionRepositoryMock.Object, _walletRepositoryMock.Object, _parkingRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_NonExistentBooking_ShouldReturnError()
        {
            // Arrange
            var bookingId = 1;
            var request = new CheckOutCommand { BookingId = bookingId };

            _bookingRepositoryMock.Setup(repo => repo.GetBookingIncludeParkingSlot(bookingId))
                .ReturnsAsync((Domain.Entities.Booking)null);



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Đơn đặt không tồn tại");
            result.StatusCode.ShouldBe(200);
        }
        [Fact]
        public async Task Handle_BookingNotInCheckOutStatus_ShouldReturnError()
        {
            // Arrange
            var bookingId = 1;
            var request = new CheckOutCommand { BookingId = bookingId };

            var booking = new Domain.Entities.Booking
            {
                BookingId = bookingId,
                Status = BookingStatus.Check_In.ToString(), // Set status to a non-Check_Out status
                UnPaidMoney = 100, // Unpaid money greater than 0, indicating it has transactions
                User = new User
                {
                    Wallet = new Wallet
                    {
                        Balance = 500 // Sufficient balance
                    }
                }
            };

            _bookingRepositoryMock.Setup(repo => repo.GetBookingIncludeParkingSlot(bookingId))
                .ReturnsAsync(booking);



            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Đơn chưa check-in hoặc đã bị hủy nên không thể xử lý.");
            result.StatusCode.ShouldBe(400);
        }
    }
}
