using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Manager.Account.RegisterCensorshipBusinessAccount.Commands.RegisterBusinessAccount;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Application.Models.PushNotification;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using Parking.FindingSlotManagement.Infrastructure.Firebase.PushService;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Parking.FindingSlotManagement.Infrastructure.HangFire
{
    public class ServiceManagement : IServiceManagement
    {
        private readonly ParkZDbContext _context;
        private readonly ILogger<ServiceManagement> _logger;
        private readonly IFireBaseMessageServices _fireBaseMessageServices;
        private readonly IEmailService _emailService;
        private readonly IHangfireRepository hangfireRepository;
        private readonly IParkingSlotRepository parkingSlotRepository;
        private readonly ITimeSlotRepository timeSlotRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IParkingRepository parkingRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IBookingDetailsRepository bookingDetailsRepository;
        private readonly IBookingRepository bookingRepository;

        public ServiceManagement(ParkZDbContext context,
            ILogger<ServiceManagement> logger,
            IFireBaseMessageServices fireBaseMessageServices,
            IEmailService emailService,
            IHangfireRepository hangfireRepository,
            IParkingSlotRepository parkingSlotRepository,
            ITimeSlotRepository timeSlotRepository,
            ITransactionRepository transactionRepository,
            IParkingRepository parkingRepository,
            IWalletRepository walletRepository,
            IBookingDetailsRepository bookingDetailsRepository,
            IBookingRepository bookingRepository)
        {
            _context = context;
            _logger = logger;
            _fireBaseMessageServices = fireBaseMessageServices;
            _emailService = emailService;
            this.hangfireRepository = hangfireRepository;
            this.parkingSlotRepository = parkingSlotRepository;
            this.timeSlotRepository = timeSlotRepository;
            this.transactionRepository = transactionRepository;
            this.parkingRepository = parkingRepository;
            this.walletRepository = walletRepository;
            this.bookingDetailsRepository = bookingDetailsRepository;
            this.bookingRepository = bookingRepository;
        }


        public async void AutoCancelBookingWhenOverAllowTimeBooking(int bookingId)
        {
            var methodName = "AutoCancelBookingWhenOverAllowTimeBooking";
            try
            {
                var bookedBooking = _context.Bookings
                    .Include(x => x.User)
                    .Include(x => x.BookingDetails)!
                        .ThenInclude(x => x.TimeSlot)
                    .Include(x => x.Transactions)
                    .FirstOrDefault(x => x.BookingId == bookingId);

                var guestArrived = bookedBooking.CheckinTime.HasValue;

                if (guestArrived)
                {
                    await hangfireRepository.DeleteJob(bookingId, methodName);
                    Console.WriteLine($"Job deleted, because guest is arrived");
                }
                else if (!guestArrived)
                {
                    if(bookedBooking.Transactions.FirstOrDefault().PaymentMethod.Equals(Domain.Enum.PaymentMethod.tra_truoc.ToString()))
                    {
                        bookedBooking.Status = BookingStatus.Cancel.ToString();
                        bookedBooking.Transactions.First().Status = BookingPaymentStatus.Huy.ToString();
                        bookedBooking.Transactions.First().Description = "Trễ quá giờ cho phép";

                    // bắn message, đơn của mày đã bị hủy + lý do
                    PushNotiToCustomerWhenOverLateAllowedTime(bookedBooking);
                    bookedBooking.User.BanCount += 1;
                    if (bookedBooking.User.BanCount >= 2)
                        foreach (var bookingDetail in bookedBooking.BookingDetails)
                        {
                            bookingDetail.TimeSlot.Status = TimeSlotStatus.Free.ToString();
                        }

                        // bắn message, đơn của mày đã bị hủy + lý do
                        PushNotiToCustomerWhenOverLateAllowedTime(bookedBooking);
                        _context.SaveChanges();
                    }
                    else
                    {
                        bookedBooking.Status = BookingStatus.Cancel.ToString();
                        bookedBooking.Transactions.First().Status = BookingPaymentStatus.Huy.ToString();
                        bookedBooking.Transactions.First().Description = "Trễ quá giờ cho phép";

                        foreach (var bookingDetail in bookedBooking.BookingDetails)
                        {
                            bookingDetail.TimeSlot.Status = TimeSlotStatus.Free.ToString();
                        }

                        // bắn message, đơn của mày đã bị hủy + lý do
                        PushNotiToCustomerWhenOverLateAllowedTime(bookedBooking);
                        bookedBooking.User.BanCount += 1;
                        if (bookedBooking.User.BanCount >= 2)
                        {
                            bookedBooking.User.IsActive = false;
                        }
                        _context.SaveChanges();
                    }
                }
                else
                {
                    Console.WriteLine($"Exception");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error at ServiceManagement.AutoCancelBookingWhenOverAllowTimeBooking: Message {ex.Message}");
            }
        }

        private void PushNotiToCustomerWhenOverLateAllowedTime(Booking? bookedBooking)
        {
            var titleCustomer = "Trạng thái đơn đặt";
            var bodyCustomer = $"Đơn của bạn đã bị hủy vì đã trễ quá 50% thời gian";

            var pushNotificationMobile = new PushNotificationMobileModel
            {
                Title = titleCustomer,
                Message = bodyCustomer,
                TokenMobile = bookedBooking.User.Devicetoken,
            };

            _fireBaseMessageServices.SendNotificationToMobileAsync(pushNotificationMobile);
        }

        public async void AutoCancelBookingWhenOutOfEndTimeBooking(int bookingId)
        {

            var methodName = "AutoCancelBookingWhenOutOfEndTimeBooking";
            try
            {
                var bookedBooking = _context.Bookings
                        .Include(x => x.User)
                        .Include(x => x.BookingDetails)!.ThenInclude(x => x.TimeSlot)
                        .Include(x => x.Transactions)!.ThenInclude(x => x.Wallet)
                        .FirstOrDefault(x => x.BookingId == bookingId);

                var guestArrived = bookedBooking.CheckinTime.Value != null;
                var isOutOfEndTimeBooking = DateTime.UtcNow.AddHours(7) >= bookedBooking.EndTime.Value;

                if (guestArrived)
                {
                    await hangfireRepository.DeleteJob(bookingId, methodName);
                    Console.WriteLine($"Job deleted, because guest is arrived");
                }
                else if (!guestArrived && isOutOfEndTimeBooking)
                {
                    bookedBooking.Status = BookingStatus.Cancel.ToString();
                    bookedBooking.Transactions.First().Status = BookingPaymentStatus.Huy.ToString();
                    bookedBooking.Transactions.First().Description = "Khách không đến";

                    foreach (var bookingDetail in bookedBooking.BookingDetails)
                    {
                        _logger.LogInformation($"Count: {bookedBooking.BookingDetails.Count}");
                        bookingDetail.TimeSlot.Status = TimeSlotStatus.Free.ToString();
                    }
                    // bắn message, đơn của mày đã bị hủy + lý do
                    PushNotiToCustomerWhenCustomerNotArrive(bookedBooking);
                    _context.SaveChanges();
                }
                Console.WriteLine($"Exception");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void PushNotiToCustomerWhenCustomerNotArrive(Booking? bookedBooking)
        {
            var titleCustomer = "Trạng thái đơn đặt";
            var bodyCustomer = "Đơn của bạn đã bị hủy vì đã quá giờ đặt";

            var pushNotificationMobile = new PushNotificationMobileModel
            {
                Title = titleCustomer,
                Message = bodyCustomer,
                TokenMobile = bookedBooking.User.Devicetoken,
            };

            _fireBaseMessageServices.SendNotificationToMobileAsync(pushNotificationMobile);
        }

        public void AddTimeSlotInFuture(int parkingSlotId)
        {
            var lstParkingSlot = _context.ParkingSlots.Where(x => x.ParkingSlotId == parkingSlotId).ToList();
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = startDate.AddDays(1);

            foreach (var a in lstParkingSlot)
            {
                List<TimeSlot> ts = new List<TimeSlot>();
                for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
                {
                    for (int i = 0; i < 24; i++)
                    {
                        DateTime startTime = date.Date + TimeSpan.FromHours(i);
                        DateTime endTime = date.Date + TimeSpan.FromHours(i + 1);

                        var entityTimeSlot = new TimeSlot
                        {
                            StartTime = startTime,
                            EndTime = endTime,
                            CreatedDate = DateTime.UtcNow.Date,
                            Status = "Free",
                            ParkingSlotId = a.ParkingSlotId
                        };
                        ts.Add(entityTimeSlot);
                    }
                }
                _context.TimeSlots.AddRange(ts);
                _context.SaveChanges();
            }
            Console.WriteLine($"Add TimeSlot In Future: Long running task {DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")}");

            var timeToDelete = DateTime.UtcNow.AddHours(7).Date.AddDays(7);

            var deleteJobId = BackgroundJob.Schedule<IServiceManagement>(x => x.UpdateTimeSlotIn1Week(parkingSlotId), timeToDelete);
            BackgroundJob.ContinueJobWith<IServiceManagement>(deleteJobId, x => x.AddTimeSlotInFuture(parkingSlotId));
            Console.WriteLine($"One week ago to update time slot: {timeToDelete}");

        }

        public void UpdateTimeSlotIn1Week(int parkingSlotId)
        {
            var listOldParkingSlot = _context.TimeSlots.Where(x => x.ParkingSlotId == parkingSlotId).ToList();

            if (listOldParkingSlot.Any())
            {
                DateTime startDate2 = DateTime.UtcNow.AddHours(7).Date;
                DateTime endDate2 = startDate2.AddDays(7);
                List<TimeSlot> test2 = new List<TimeSlot>();
                for (DateTime date = startDate2; date < endDate2; date = date.AddDays(1))
                {
                    for (int i = 0; i < 24; i++)
                    {
                        DateTime startTime = date.Date + TimeSpan.FromHours(i);
                        DateTime endTime = date.Date + TimeSpan.FromHours(i + 1);
                        TimeSlot x = new TimeSlot()
                        {
                            StartTime = startTime,
                            EndTime = endTime
                        };
                        test2.Add(x);
                    }
                }
                for (int i = 0; i < listOldParkingSlot.Count && i < test2.Count; i++)
                {
                    listOldParkingSlot[i].StartTime = test2[i].StartTime;
                    listOldParkingSlot[i].EndTime = test2[i].EndTime;
                    listOldParkingSlot[i].CreatedDate = DateTime.UtcNow.AddHours(7).Date;
                }
                _context.TimeSlots.UpdateRange(listOldParkingSlot);
                _context.SaveChanges();
                Console.WriteLine($"Update TimeSlot In One Week: Long running task {DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")}");
                var timeToDelete = DateTime.UtcNow.AddHours(7).AddDays(7).Date - DateTime.UtcNow.AddHours(7);

                var deleteJobId = BackgroundJob.Schedule<IServiceManagement>(x => x.UpdateTimeSlotIn1Week(parkingSlotId), timeToDelete);
                Console.WriteLine($"One week ago to update time slot: {timeToDelete}");
            }
            else
            {
                Console.WriteLine($"có gì đâu mà update");
            }

        }

        public void GenerateMerchandise()
        {
            Console.WriteLine($"Generate Merchandise: Long running task {DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void SendEmail(int entity)
        {
            Console.WriteLine("blabla send email");
            Console.WriteLine($"Send Email: Long running task {DateTime.UtcNow.AddHours(7).ToString("dd-MM-yyyy HH:mm:ss")}");
        }

        public void SyncData()
        {
            Console.WriteLine($"Sync Data: Long running task {DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void ChargeMoneyFor1MonthUsingSystem(Fee fee, int bussinesId, int billId, User user)
        {
            try
            {
                var adminWallet = _context.Wallets.FirstOrDefault(x => x.WalletId == 1);
                var userWallet = _context.Wallets.FirstOrDefault(x => x.UserId == user.UserId);
                var billExist = _context.Bills.FirstOrDefault(x => x.BillId == billId);
                if (userWallet.Balance >= fee.Price)
                {
                    userWallet.Balance -= fee.Price;
                }
                else if (userWallet.Balance < fee.Price)
                {
                    userWallet.Balance -= fee.Price;
                    if (userWallet.Balance < 0)
                    {
                        userWallet.Debt += userWallet.Balance;
                    }

                }
                adminWallet.Balance += fee.Price;
                billExist.WalletId = userWallet.WalletId;
                billExist.Status = BillStatus.Đã_Thanh_Toán.ToString();
                _context.SaveChanges();
                var newBill = new Bill()
                {
                    Time = DateTime.UtcNow.AddHours(7),
                    Status = BillStatus.Chờ_Thanh_Toán.ToString(),
                    Price = fee.Price,
                    BusinessId = bussinesId
                };
                _context.Bills.Add(newBill);
                _context.SaveChanges();

                Transaction transaction = new Transaction()
                {
                    Price = fee.Price,
                    WalletId = userWallet.WalletId,
                    PaymentMethod = PaymentMethod.thanh_toan_online.ToString(),
                    Status = BillStatus.Đã_Thanh_Toán.ToString(),
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    Description = "Hệ thống trừ tiền sử dụng dịch vụ"
                };
                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                SendMailToManager(fee, user);
                SetNewJob(fee, bussinesId, user, newBill);

                //RecurringJob.AddOrUpdate<IServiceManagement>(x => x.ChargeMoneyFor1MonthUsingSystem(fee, bussinesId, newBill.BillId, user), Cron.MinuteInterval(6));
            }
            catch (Exception ex)
            {
                throw new Exception("Hangfire:" + ex.Message);
            }

        }
        private void SendMailToManager(Fee fee, User user)
        {
            EmailModel emailModel = new EmailModel();
            emailModel.To = user.Email;
            emailModel.Subject = "Thông báo: Trừ tiền từ tài khoản ví của bạn";
            System.Net.Mail.MailMessage message = new();
            message.Body = $"Dear {user.Name}, " + Environment.NewLine;
            message.Body += "Chúng tôi xin thông báo rằng hệ thống của chúng tôi đã trừ một khoản tiền từ tài khoản ví của bạn." + Environment.NewLine;
            message.Body += $"Số tiền đã trừ: {fee.Price} đồng" + Environment.NewLine;
            message.Body += $"Ngày trừ tiền: {DateTime.UtcNow.AddHours(7)}" + Environment.NewLine;
            message.Body += $"Gói sử dụng: {fee.Name}" + Environment.NewLine;
            message.Body += "Xin lưu ý rằng việc trừ tiền này đảm bảo bạn tiếp tục sử dụng các tính năng và dịch vụ của hệ thống chúng tôi." + Environment.NewLine;
            message.Body += "Nếu bạn có bất kỳ câu hỏi hoặc cần hỗ trợ thêm, xin vui lòng liên hệ với chúng tôi qua địa chỉ email hoặc số điện thoại dưới đây. Chúng tôi luôn sẵn sàng hỗ trợ bạn." + Environment.NewLine;
            message.Body += "Chân thành cảm ơn sự tin tưởng và ủng hộ của bạn đối với hệ thống của chúng tôi." + Environment.NewLine;
            message.Body += "Trân trọng," + Environment.NewLine;
            message.Body += "ParkZ" + Environment.NewLine;
            message.Body += "Địa chỉ công ty: Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000" + Environment.NewLine;
            message.Body += "Số điện thoại công ty: 0793808821" + Environment.NewLine;
            message.Body += "Địa chỉ email công ty: parkz.thichthicodeteam@gmail.com" + Environment.NewLine;
            emailModel.Body = message.Body;
            _emailService.SendMail(emailModel);
            Console.WriteLine("done email");
        }

        private static void SetNewJob(Fee fee, int bussinesId, User user, Bill newBill)
        {
            var timeToCancel = DateTime.UtcNow.AddDays(1);

            BackgroundJob.Schedule<IServiceManagement>(
                x => x.ChargeMoneyFor1MonthUsingSystem(fee, bussinesId, newBill.BillId, user), timeToCancel);
        }

        public async void CheckIfBookingIsLateOrNot(int bookingId, int parkingId, List<string> token, User ManagerOfParking)
        {
            Console.WriteLine("Background Job CheckIfBookingIsLateOrNot Called");

            var newJobId = "";
            var bookedBooking = _context.Bookings
                                        .Include(x => x.BookingDetails!).ThenInclude(x => x.TimeSlot)
                                        .FirstOrDefault(x => x.BookingId == bookingId);

            var customerIsCheckOut = bookedBooking.CheckoutTime != null;
            var customerIsCheckIn = bookedBooking.CheckinTime.HasValue;

            var bookedTimeSlot = bookedBooking.BookingDetails.Last().TimeSlotId;

            var nextTimeSlot = _context.TimeSlots.Find(bookedTimeSlot + 1);
            var methodName = "CheckIfBookingIsLateOrNot";

            //Delete job
            if (customerIsCheckOut && customerIsCheckIn)
            {
                await hangfireRepository.DeleteJob(bookingId, methodName);
                Console.WriteLine("job deleted, because customer is CheckOut");
            }
            if (!customerIsCheckOut && nextTimeSlot.Status.Equals(TimeSlotStatus.Free.ToString()) && customerIsCheckIn)
            {
                bookedBooking.Status = BookingStatus.OverTime.ToString();

                nextTimeSlot.Status = TimeSlotStatus.Booked.ToString();
                var newBookingDetail = new BookingDetails
                {
                    BookingId = bookingId,
                    TimeSlotId = nextTimeSlot.TimeSlotId
                };

                _context.BookingDetails.Add(newBookingDetail);
                _context.SaveChanges();

                DateTime end = DateTime.Parse(nextTimeSlot.EndTime.ToString()).AddMinutes(1);
                DateTimeOffset timeToCallMethod = new DateTimeOffset(end, new TimeSpan(7, 0, 0));
                newJobId = BackgroundJob.Schedule<IServiceManagement>(
                x => x.CheckIfBookingIsLateOrNot(bookingId, parkingId, token, ManagerOfParking),
                timeToCallMethod);
            }
            else if (!customerIsCheckOut && nextTimeSlot.Status.Equals(TimeSlotStatus.Booked.ToString()) && customerIsCheckIn)
            {
                Console.WriteLine("Background Job: co request can xu ly");
                bookedBooking.Status = BookingStatus.OverTime.ToString();

                var conflictBookingDetails = _context.BookingDetails
                .FirstOrDefault(x => x.TimeSlotId == nextTimeSlot.TimeSlotId);

                var newConflictRequest = new ConflictRequest
                {
                    BookingId = (int)conflictBookingDetails.BookingId,
                    Message = $"{ConflictRequestMessage.Qua_gio.ToString()} - Có chỗ đễ xe cần chuyển",
                    ParkingId = parkingId,
                    Status = ConflictRequestStatus.InProcess.ToString(),
                };

                _context.ConflictRequests.Add(newConflictRequest);
                conflictBookingDetails.BookingId = bookingId;
                _context.SaveChanges();

                DateTime end = DateTime.Parse(nextTimeSlot.EndTime.ToString()).AddMinutes(1);
                DateTimeOffset timeToCallMethod = new DateTimeOffset(end, new TimeSpan(7, 0, 0));
                newJobId = BackgroundJob.Schedule<IServiceManagement>(
                x => x.CheckIfBookingIsLateOrNot(bookingId, parkingId, token, ManagerOfParking),
                timeToCallMethod);

                if (token.Any())
                {
                    foreach (var item in token)
                    {
                        var pushNotificationModel = new PushNotificationWebModel
                        {
                            Title = "Đơn đặt xung đột, cần xử lý",
                            Message = "Vui lòng kiểm tra thông báo,......",
                            TokenWeb = item,
                        };
                        _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
                    }
                }
                else
                {
                    var manager = _context.Users.Find(ManagerOfParking.UserId!);
                    var pushNotificationModel = new PushNotificationWebModel
                    {
                        Title = "Đơn đặt xung đột, cần xử lý",
                        Message = "Vui lòng kiểm tra thông báo,......",
                        TokenWeb = manager.Devicetoken,
                    };
                    _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
                }
            }
        }

        public async Task DisableParkingAtDate(int parkingId)
        {
            var parkingIncludeTimeSlots = await _context.Parkings
                .Include(x => x.Floors)!.ThenInclude(x => x.ParkingSlots)!.ThenInclude(x => x.TimeSlots)
                .FirstOrDefaultAsync(x => x.ParkingId == parkingId);

            var floors = parkingIncludeTimeSlots!.Floors!;
            parkingIncludeTimeSlots.IsAvailable = false;
        }

        public async Task DisableParkingByDate(int parkingId, DateTime disableDate, string reason)
        {
            try
            {
                var parkingIncludeTimeSlots = await _context.Parkings
                    .Include(x => x.Floors)!.ThenInclude(x => x.ParkingSlots)!.ThenInclude(x => x.TimeSlots)
                    .FirstOrDefaultAsync(x => x.ParkingId == parkingId);

                var floors = parkingIncludeTimeSlots!.Floors!;
                parkingIncludeTimeSlots.IsAvailable = false;

                var parkingSlots = await parkingSlotRepository.GetParkingSlotsByParkingId(parkingId);
                var bookedTimeSlotsAtDisableDate = await timeSlotRepository.GetBookedTimeSlotsByDateNew(parkingSlots.ToList(), disableDate);
                if (bookedTimeSlotsAtDisableDate != null)
                {
                    var tempbookingDetails = new List<BookingDetails>();
                    foreach (var item in bookedTimeSlotsAtDisableDate)
                    {

                        var bookingDetails = await bookingDetailsRepository.GetBookingDetailsByTimeSlotId(item.ToList());

                        foreach (var bookingDetail in bookingDetails)
                        {
                            if (!tempbookingDetails.Any(x => x.BookingId == bookingDetail.BookingId))
                            {
                                tempbookingDetails.Add(bookingDetail);
                            }
                        }

                        var reasonChangeTransactionStatus = reason;

                        var prePaidTransactions = await transactionRepository.GetPrePaidTransactions(tempbookingDetails.ToList());
                        foreach (var prePaidTransaction in prePaidTransactions)
                        {
                            var paidMoney = prePaidTransaction.Price;
                            var customerWallet = prePaidTransaction.Wallet!;
                            var parkingManagerId = await parkingRepository.GetManagerIdByParkingId(parkingId);
                            var managerWallet = await walletRepository.GetWalletById(parkingManagerId);

                            customerWallet.Balance += paidMoney;
                            managerWallet.Balance -= paidMoney;

                            Transaction billTrans = new Transaction()
                            {
                                BookingId = prePaidTransaction.BookingId,
                                CreatedDate = DateTime.UtcNow.AddHours(7),
                                Description = "Hoàn tiền",
                                Price = paidMoney,
                                WalletId = customerWallet.WalletId,
                                PaymentMethod = prePaidTransaction.PaymentMethod.ToString(),
                                Status = prePaidTransaction.Status.ToString()
                            };


                            Transaction billTransManager = new Transaction()
                            {
                                BookingId = prePaidTransaction.BookingId,
                                CreatedDate = DateTime.UtcNow.AddHours(7),
                                Description = "Hoàn tiền cho khách hàng",
                                Price = paidMoney,
                                WalletId = managerWallet.WalletId,
                                PaymentMethod = prePaidTransaction.PaymentMethod.ToString(),
                                Status = prePaidTransaction.Status.ToString()
                            };

                            await transactionRepository.Insert(billTrans);
                            await transactionRepository.Insert(billTransManager);

                            await walletRepository.Save();
                        }

                        // await transactionRepository.ChangeStatusOriginalTransactionsByBookingDetail(tempbookingDetails.ToList(), reasonChangeTransactionStatus);
                        await bookingRepository.CancelBookedBookingWhenDisableParking(tempbookingDetails.ToList());
                        await timeSlotRepository.DisableTimeSlotByDisableDate(parkingSlots.ToList(), disableDate);
                        // Bắn message chưa có token, lỗi, ko for típ dc nên comment
                        // await PushNotiForAllCustomer(tempbookingDetails, reasonChangeTransactionStatus);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at ServiceManagement.DisableParkingByDate: Message {ex.Message}");
            }
        }
    }
}
