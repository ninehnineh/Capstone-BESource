using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CancelBooking;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Customer.Booking.Commands
{
    public class CancelBookingCommandHandlerTest
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IParkingSlotRepository> _parkingSlotRepositoryMock;
        private readonly Mock<IBookingDetailsRepository> _bookingDetailsRepositoryMock;
        private readonly Mock<ITimeSlotRepository> _timeSlotRepositoryMock;
        private readonly Mock<IHangfireRepository> _hangfireRepositoryMock;
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly CancelBookingCommandHandler _handler;
        public CancelBookingCommandHandlerTest()
        {
            _bookingDetailsRepositoryMock = new Mock<IBookingDetailsRepository> ();
            _bookingRepositoryMock = new Mock<IBookingRepository> ();
            _parkingSlotRepositoryMock = new Mock<IParkingSlotRepository> ();
            _timeSlotRepositoryMock = new Mock<ITimeSlotRepository> ();
            _hangfireRepositoryMock = new Mock<IHangfireRepository>();
            _walletRepositoryMock = new Mock<IWalletRepository> ();
            _userRepositoryMock = new Mock<IUserRepository> ();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _handler = new CancelBookingCommandHandler(_bookingRepositoryMock.Object, _parkingSlotRepositoryMock.Object, _bookingDetailsRepositoryMock.Object, _timeSlotRepositoryMock.Object, _hangfireRepositoryMock.Object, _walletRepositoryMock.Object, _userRepositoryMock.Object, _transactionRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_BookingStatusSuccess_CancelsBooking_Tra_sau()
        {
            var request = new CancelBookingCommand()
            {
                BookingId = 1,

            };
            var booking = new Domain.Entities.Booking { BookingId = 1, Status = BookingStatus.Success.ToString(), Transactions = new List<Domain.Entities.Transaction> { new Domain.Entities.Transaction { BookingId = 1, TransactionId = 1, PaymentMethod = Domain.Enum.PaymentMethod.tra_sau.ToString(), Status = TransactionStatus.Chua_thanh_toan.ToString() } }};
            _bookingRepositoryMock.Setup(x => x.GetBookingIncludeParkingSlot(1)).ReturnsAsync(booking);

            var bookingDetail = new List<BookingDetails> { new BookingDetails { BookingId = 1, TimeSlotId = 1, BookingDetailsId = 1, TimeSlot = new TimeSlot() { TimeSlotId = 1, ParkingSlotId = 1, Status = TimeSlotStatus.Booked.ToString() } } };
            var parkingSlot = new Domain.Entities.ParkingSlot() { ParkingSlotId = 1, FloorId = 1 };
            var floor = new Floor() { FloorId = 1, ParkingId = 1 };
            var parking = new Domain.Entities.Parking() { ParkingId = 1, BusinessId = 1 };
            var businessProfile = new Domain.Entities.BusinessProfile() { BusinessProfileId = 1 };

            _bookingDetailsRepositoryMock.Setup(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BookingDetails, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.BookingDetails, object>>>>(), null, false)).ReturnsAsync(bookingDetail);
            var managerExist = new User { UserId = 1};
            var walletOfManaer = new Domain.Entities.Wallet { UserId = 1, WalletId = 1, Balance = 100 };
            _userRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.User, object>>>>(), false)).ReturnsAsync(managerExist);
            
            var result = await _handler.Handle(request, CancellationToken.None);


            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Hủy chỗ đặt thành công");
            booking.Status.ShouldBe(BookingStatus.Cancel.ToString());
            bookingDetail.FirstOrDefault().TimeSlot.Status.ShouldBe(TimeSlotStatus.Free.ToString());
        }
        [Fact]
        public async Task Handle_BookingStatusSuccess_CancelsBooking_Tra_truoc()
        {
            var request = new CancelBookingCommand()
            {
                BookingId = 1,

            };
            var booking = new Domain.Entities.Booking { BookingId = 1, Status = BookingStatus.Success.ToString(), User = new User { UserId = 2, Wallet = new Domain.Entities.Wallet {WalletId = 2, UserId = 2, Balance = 50 } }, Transactions = new List<Domain.Entities.Transaction> { new Domain.Entities.Transaction { BookingId = 1, TransactionId = 1, PaymentMethod = Domain.Enum.PaymentMethod.tra_truoc.ToString(), Status = TransactionStatus.Da_thanh_toan.ToString(), Price = 50 } } };
            _bookingRepositoryMock.Setup(x => x.GetBookingIncludeParkingSlot(1)).ReturnsAsync(booking);

            var bookingDetail = new List<BookingDetails> { new BookingDetails { BookingId = 1, TimeSlotId = 1, BookingDetailsId = 1, TimeSlot = new TimeSlot() { TimeSlotId = 1, ParkingSlotId = 1, Status = TimeSlotStatus.Booked.ToString() } } };
            var parkingSlot = new Domain.Entities.ParkingSlot() { ParkingSlotId = 1, FloorId = 1 };
            var floor = new Floor() { FloorId = 1, ParkingId = 1 };
            var parking = new Domain.Entities.Parking() { ParkingId = 1, BusinessId = 1 };
            var businessProfile = new Domain.Entities.BusinessProfile() { BusinessProfileId = 1 };

            _bookingDetailsRepositoryMock.Setup(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BookingDetails, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.BookingDetails, object>>>>(), null, false)).ReturnsAsync(bookingDetail);
            var managerExist = new User { UserId = 1, Wallet = new Domain.Entities.Wallet { UserId = 1, WalletId = 1, Balance = 100 } };
            _userRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.User, object>>>>(), false)).ReturnsAsync(managerExist);

            var result = await _handler.Handle(request, CancellationToken.None);


            result.Success.ShouldBeTrue();
            result.StatusCode.ShouldBe(200);
            result.Message.ShouldBe("Hủy chỗ đặt thành công");
            booking.Status.ShouldBe(BookingStatus.Cancel.ToString());
            bookingDetail.FirstOrDefault().TimeSlot.Status.ShouldBe(TimeSlotStatus.Free.ToString());
            managerExist.Wallet.Balance.ShouldBe(50); // 100 - 50
        }
        [Fact]
        public async Task Handle_BookingStatusNotSuccess_ReturnsErrorMessage()
        {
            var request = new CancelBookingCommand()
            {
                BookingId = 1,

            };
            var booking = new Domain.Entities.Booking { BookingId = 1, Status = BookingStatus.Check_In.ToString(), User = new User { UserId = 2, Wallet = new Domain.Entities.Wallet { WalletId = 2, UserId = 2, Balance = 50 } }, Transactions = new List<Domain.Entities.Transaction> { new Domain.Entities.Transaction { BookingId = 1, TransactionId = 1, PaymentMethod = Domain.Enum.PaymentMethod.tra_truoc.ToString(), Status = TransactionStatus.Da_thanh_toan.ToString(), Price = 50 } } };
            _bookingRepositoryMock.Setup(x => x.GetBookingIncludeParkingSlot(1)).ReturnsAsync(booking);

            var bookingDetail = new List<BookingDetails> { new BookingDetails { BookingId = 1, TimeSlotId = 1, BookingDetailsId = 1, TimeSlot = new TimeSlot() { TimeSlotId = 1, ParkingSlotId = 1, Status = TimeSlotStatus.Booked.ToString() } } };
            var parkingSlot = new Domain.Entities.ParkingSlot() { ParkingSlotId = 1, FloorId = 1 };
            var floor = new Floor() { FloorId = 1, ParkingId = 1 };
            var parking = new Domain.Entities.Parking() { ParkingId = 1, BusinessId = 1 };
            var businessProfile = new Domain.Entities.BusinessProfile() { BusinessProfileId = 1 };

            _bookingDetailsRepositoryMock.Setup(x => x.GetAllItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.BookingDetails, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.BookingDetails, object>>>>(), null, false)).ReturnsAsync(bookingDetail);
            var managerExist = new User { UserId = 1, Wallet = new Domain.Entities.Wallet { UserId = 1, WalletId = 1, Balance = 100 } };
            _userRepositoryMock.Setup(x => x.GetItemWithCondition(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>(), It.IsAny<List<Expression<Func<Domain.Entities.User, object>>>>(), false)).ReturnsAsync(managerExist);

            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.ShouldBeFalse();
            result.StatusCode.ShouldBe(400);
            result.Message.ShouldBe("Đơn đang ở trạng thái khác hoặc đã bị hủy nên không thể xử lý.");
            _bookingRepositoryMock.Verify(x => x.Save(), Times.Never);
            _timeSlotRepositoryMock.Verify(x => x.Save(), Times.Never);
            _walletRepositoryMock.Verify(x => x.Save(), Times.Never);
        }
    }
}
