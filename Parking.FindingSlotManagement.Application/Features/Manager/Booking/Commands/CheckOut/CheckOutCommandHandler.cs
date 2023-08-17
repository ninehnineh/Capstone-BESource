using MediatR;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using System.Linq.Expressions;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.CheckOut
{
    public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, ServiceResponse<string>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFireBaseMessageServices _fireBaseMessageServices;
        private readonly IConfiguration _configuration;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IParkingRepository _parkingRepository;

        public CheckOutCommandHandler(IBookingRepository bookingRepository,
            IFireBaseMessageServices fireBaseMessageServices,
            IConfiguration configuration,
            ITransactionRepository transactionRepository,
            IWalletRepository walletRepository,
            IParkingRepository parkingRepository)
        {
            _bookingRepository = bookingRepository;
            _fireBaseMessageServices = fireBaseMessageServices;
            _configuration = configuration;
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
            _parkingRepository = parkingRepository;
        }

        public async Task<ServiceResponse<string>> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            var checkOutTime = DateTime.UtcNow.AddHours(7);
            var parkingId = request.ParkingId;
            var paymentMethod = request.PaymentMethod;
            //var moneyCustomerMustPayAfterCheckOut = request.TotalPrice;
            var bookingId = request.BookingId;

            try
            {

                var booking = await _bookingRepository
                    .GetBookingIncludeParkingSlot(bookingId);

                if (booking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Đơn đặt không tồn tại",
                        StatusCode = 200,
                        Success = false
                    };
                }
                if (!booking.Status.Equals(BookingStatus.Check_Out.ToString()))
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Đơn chưa check-in hoặc đã bị hủy nên không thể xử lý.",
                        StatusCode = 400,
                        Success = false
                    };
                }
                var includesss = new List<Expression<Func<Domain.Entities.Parking, object>>>
                {
                    x => x.BusinessProfile.User.Wallet
                };

                var moneyCustomerMustPayAfterCheckOut = booking.UnPaidMoney;

                var parking = await _parkingRepository
                    .GetItemWithCondition(x => x.ParkingId == parkingId, includesss, false);

                if (parking == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Bãi xe không tồn tại",
                        StatusCode = 200,
                        Success = false
                    };
                }
                // Đã thanh toán trước, vào bãi, ra bãi đúng giờ, chỉ có một transaction
                else if (moneyCustomerMustPayAfterCheckOut == 0 &&
                    string.IsNullOrEmpty(paymentMethod))
                {

                    booking.Status = BookingStatus.Done.ToString();
                    await _bookingRepository.Save();
                }

                // Có nhiều transactions, và thanh toán online qua ví
                else if (moneyCustomerMustPayAfterCheckOut > 0 &&
                    paymentMethod.Equals(PaymentMethod.thanh_toan_online.ToString()))
                {

                    if (booking.User.Wallet.Balance < moneyCustomerMustPayAfterCheckOut)
                    {
                        return new ServiceResponse<string>
                        {
                            Message = "Số dư không đủ, vui lòng nạp thêm.",
                            StatusCode = 400,
                            Success = false
                        };
                    }

                    booking.User.Wallet.Balance -= (decimal)moneyCustomerMustPayAfterCheckOut;
                    parking.BusinessProfile.User.Wallet.Balance += (decimal)moneyCustomerMustPayAfterCheckOut;
                    await _walletRepository.Save();

                    var transactions = booking.Transactions;

                    var sumPrice = 0M;
                    foreach (var transaction in transactions)
                    {
                        transaction.Status = BookingPaymentStatus.Da_thanh_toan.ToString();
                        transaction.PaymentMethod = PaymentMethod.thanh_toan_online.ToString();
                        transaction.WalletId = booking.User.Wallet.WalletId;
                        sumPrice += transaction.Price;
                    }
                    await _transactionRepository.Save();

                    Transaction transactionForManager = new()
                    {
                        Price = moneyCustomerMustPayAfterCheckOut,
                        Status = BookingPaymentStatus.Da_thanh_toan.ToString(),
                        PaymentMethod = PaymentMethod.thanh_toan_online.ToString(),
                        Description = "Nhận tiền thanh toán từ đơn đặt có Mã: " + booking.BookingId,
                        WalletId = parking.BusinessProfile.User.Wallet.WalletId
                    };
                    await _transactionRepository.Insert(transactionForManager);

                    booking.Status = BookingStatus.Done.ToString();
                    booking.TotalPrice = sumPrice;
                    booking.UnPaidMoney = 0;
                    await _bookingRepository.Save();
                }

                // Có nhiều transactions, và thanh toán tiền mặt
                else if (moneyCustomerMustPayAfterCheckOut > 0 &&
                    paymentMethod.Equals(PaymentMethod.thanh_toan_tien_mat.ToString()))
                {
                    var transactions = booking.Transactions;

                    var sumPrice = 0M;
                    foreach (var transaction in transactions)
                    {
                        if (transaction.Status == BookingPaymentStatus.Chua_thanh_toan.ToString())
                        {
                            transaction.Status = BookingPaymentStatus.Da_thanh_toan.ToString();
                            transaction.PaymentMethod = PaymentMethod.thanh_toan_tien_mat.ToString();
                        }
                        sumPrice += transaction.Price;
                    }
                    await _transactionRepository.Save();

                    Transaction transactionForManager = new()
                    {
                        Price = moneyCustomerMustPayAfterCheckOut,
                        Status = BookingPaymentStatus.Da_thanh_toan.ToString(),
                        PaymentMethod = PaymentMethod.thanh_toan_tien_mat.ToString(),
                        Description = "Nhận tiền thanh toán từ đơn đặt có Mã: " + booking.BookingId,
                        WalletId = parking.BusinessProfile.User.Wallet.WalletId
                    };
                    await _transactionRepository.Insert(transactionForManager);

                    booking.Status = BookingStatus.Done.ToString();
                    booking.TotalPrice = sumPrice;
                    booking.UnPaidMoney = 0;
                    await _bookingRepository.Save();
                }


                var titleCustomer = _configuration.GetSection("MessageTitle_Customer")
                    .GetSection("CheckOut").Value;
                var bodyCustomer = _configuration.GetSection("MessageBody_Customer")
                    .GetSection("CheckOut").Value;
                if (booking.User == null)
                {
                    return new ServiceResponse<string>
                    {
                        StatusCode = 204,
                        Message = "Thành công",
                        Success = true,
                    };
                }
                var userDiviceToken = booking.User!.Devicetoken;

                if (userDiviceToken == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Thành công",
                        StatusCode = 201,
                        Success = true,
                    };
                }
                else
                {
                    var pushNotificationMobile = new PushNotificationMobileModel
                    {
                        Title = titleCustomer,
                        Message = bodyCustomer,
                        TokenMobile = userDiviceToken,
                    };

                    await _fireBaseMessageServices
                        .SendNotificationToMobileAsync(pushNotificationMobile);
                }
                return new ServiceResponse<string>
                {
                    StatusCode = 204,
                    Message = "Thành công",
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
