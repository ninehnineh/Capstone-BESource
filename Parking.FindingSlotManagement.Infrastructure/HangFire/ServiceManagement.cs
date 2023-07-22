using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
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

namespace Parking.FindingSlotManagement.Infrastructure.HangFire
{
    public class ServiceManagement : IServiceManagement
    {
        private readonly ParkZDbContext _context;
        private readonly ILogger<ServiceManagement> _logger;
        private readonly IFireBaseMessageServices _fireBaseMessageServices;
        private readonly IEmailService _emailService;

        public ServiceManagement(ParkZDbContext context,
            ILogger<ServiceManagement> logger,
            IFireBaseMessageServices fireBaseMessageServices,
            IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _fireBaseMessageServices = fireBaseMessageServices;
            _emailService = emailService;
        }





        public void AutoCancelBookingWhenOverAllowTimeBooking(int bookingId)
        {
            var lateHoursAllowed = 1;

            try
            {
                var bookedBooking = _context.Bookings
                    .Include(x => x.BookingDetails)!
                        .ThenInclude(x => x.TimeSlot)
                    .Include(x => x.Transactions)
                    .FirstOrDefault(x => x.BookingId == bookingId);

                var guestArrived = bookedBooking.CheckinTime.HasValue;

                if (!guestArrived)
                {
                    bookedBooking.Status = BookingStatus.Cancel.ToString();
                    bookedBooking.Transactions.First().Status = BookingPaymentStatus.Huy.ToString();
                    bookedBooking.Transactions.First().Description = "Trễ quá giờ cho phép";

                    foreach (var bookingDetail in bookedBooking.BookingDetails)
                    {
                        _logger.LogInformation($"Count: {bookedBooking.BookingDetails.Count}");
                        bookingDetail.TimeSlot.Status = TimeSlotStatus.Free.ToString();
                    }

                    // bắn message, đơn của mày đã bị hủy + lý do

                    //PushNotiToCustomerWhenOverLateAllowedTime(lateHoursAllowed, bookedBooking);

                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"Exception");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void PushNotiToCustomerWhenOverLateAllowedTime(int lateHoursAllowed, Booking? bookedBooking)
        {
            var titleCustomer = "Trạng thái đơn đặt";
            var bodyCustomer = $"Đơn của bạn đã bị hủy vì đã quá giờ trễ cho phép ({lateHoursAllowed})";

            var pushNotificationMobile = new PushNotificationMobileModel
            {
                Title = titleCustomer,
                Message = bodyCustomer,
                TokenMobile = bookedBooking.User.Devicetoken,
            };

            _fireBaseMessageServices.SendNotificationToMobileAsync(pushNotificationMobile);
        }

        public void AutoCancelBookingWhenOutOfEndTimeBooking(int bookingId)
        {
            try
            {
                var bookedBooking = _context.Bookings
                        .Include(x => x.User)
                        .Include(x => x.BookingDetails)!.ThenInclude(x => x.TimeSlot)
                        .Include(x => x.Transactions)!.ThenInclude(x => x.Wallet)
                        .FirstOrDefault(x => x.BookingId == bookingId);

                var guestArrived = bookedBooking.CheckinTime.HasValue;
                var isOutOfEndTimeBooking = DateTime.UtcNow.AddHours(7) >= bookedBooking.EndTime.Value;

                if (!guestArrived && isOutOfEndTimeBooking)
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
                    //PushNotiToCustomerWhenCustomerNotArrive(bookedBooking);
                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"Exception");
                }
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
            DateTime endDate = startDate.AddDays(7);

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

            var timeToDelete = DateTime.UtcNow.AddDays(7);

            var deleteJobId = BackgroundJob.Schedule<IServiceManagement>(x => x.DeleteTimeSlotIn1Week(), timeToDelete);
            BackgroundJob.ContinueJobWith<IServiceManagement>(deleteJobId, x => x.AddTimeSlotInFuture(parkingSlotId));
            Console.WriteLine($"One week ago to delete time slot: {timeToDelete}");

        }

        public void DeleteTimeSlotIn1Week()
        {
            var oneWeekAgo = DateTime.UtcNow.AddHours(7).AddDays(-7);
            Console.WriteLine($"One week ago to delete time slot: {oneWeekAgo}");
            var dataToDelete = _context.TimeSlots.Where(x => x.CreatedDate <= oneWeekAgo);
            if (dataToDelete.Any())
            {
                _context.TimeSlots.RemoveRange(dataToDelete);
                _context.SaveChanges();
                Console.WriteLine($"Delete TimeSlot In One Week: Long running task {DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            else
            {
                Console.WriteLine($"có đéo gì đâu mà xóa");
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

        public void CheckIfBookingIsLateOrNot(int bookingId, int parkingId, List<string> token, User ManagerOfParking, string jobId)
        {
            Console.WriteLine("Background Job CheckIfBookingIsLateOrNot Called");
            var bookedBooking = _context.Bookings
                                        .Include(x => x.BookingDetails!).ThenInclude(x => x.TimeSlot)
                                        .FirstOrDefault(x => x.BookingId == bookingId);

            var customerIsCheckOut = bookedBooking.CheckoutTime != null;
            var bookedTimeSlot = bookedBooking.BookingDetails.Last().TimeSlotId;

            var nextTimeSlot = _context.TimeSlots.Find(bookedTimeSlot + 1);


            //Delete job
            if (customerIsCheckOut)
            {
                BackgroundJob.Delete(jobId);
            }
            else if (!customerIsCheckOut && nextTimeSlot.Status.Equals(TimeSlotStatus.Free.ToString()))
            {
                var newJobId = "";
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
                x => x.CheckIfBookingIsLateOrNot(bookingId, parkingId, token, ManagerOfParking, newJobId),
                timeToCallMethod);
            }
            else if (!customerIsCheckOut && nextTimeSlot.Status.Equals(TimeSlotStatus.Booked.ToString()))
            {
                Console.WriteLine("Background Job: co request can xu ly");
                var conflictBooking = _context.BookingDetails.FirstOrDefault(x => x.TimeSlotId == nextTimeSlot.TimeSlotId);

                var newConflictRequest = new ConflictRequest
                {
                    BookingId = (int)conflictBooking.BookingId,
                    Message = "Có request cần xử lý",
                    ParkingId = parkingId,
                    Status = "Process",
                };

                _context.ConflictRequests.Add(newConflictRequest);
                _context.SaveChanges();

                if (token.Any())
                {
                    foreach (var item in token)
                    {
                        var pushNotificationModel = new PushNotificationWebModel
                        {
                            // Title = titleManager,
                            // Message = bodyManager + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
                            TokenWeb = item,
                        };
                        _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
                    }
                }
                else
                {
                    var manager =  _context.Users.Find(ManagerOfParking.UserId!);
                    var pushNotificationModel = new PushNotificationWebModel
                    {
                        // Title = titleManager,
                        // Message = bodyManager + "Vị trí " + floor.FloorName + "-" + parkingSlot.Name,
                        TokenWeb = manager.Devicetoken,
                    };
                    _fireBaseMessageServices.SendNotificationToWebAsync(pushNotificationModel);
                }
            }
        }
    }
}
