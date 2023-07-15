using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
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

        public void AddTimeSlotInFuture(int floorId)
        {
            var lstParkingSlot = _context.ParkingSlots.Where(x => x.FloorId == floorId).ToList();
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
            RecurringJob.AddOrUpdate<IServiceManagement>(x => x.DeleteTimeSlotIn1Week(), Cron.Weekly);
            RecurringJob.AddOrUpdate<IServiceManagement>(x => x.AddTimeSlotInFuture((int)floorId), Cron.Weekly);

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

        public void DeleteTimeSlotIn1Week()
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
            var dataToDelete = _context.TimeSlots.Where(x => x.CreatedDate < oneWeekAgo);

            _context.TimeSlots.RemoveRange(dataToDelete);
            _context.SaveChanges();
            Console.WriteLine($"Delete TimeSlot In One Week: Long running task {DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void GenerateMerchandise()
        {
            Console.WriteLine($"Generate Merchandise: Long running task {DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void SendEmail()
        {
            Console.WriteLine($"Send Email: Long running task {DateTime.UtcNow.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss")}");
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
                EmailModel emailModel = new EmailModel();
                emailModel.To = user.Email;
                emailModel.Subject = "Thông báo: Trừ tiền từ tài khoản ví của bạn";

                string body = $"Dear {user.Name},\n\n";
                body += "Chúng tôi xin thông báo rằng hệ thống của chúng tôi đã trừ một khoản tiền từ tài khoản ví của bạn.\n";
                body += $"Số tiền đã trừ: {fee.Price} đồng\n";
                body += $"Ngày trừ tiền: {DateTime.UtcNow.AddHours(7)}\n";
                body += $"Gói sử dụng: {fee.Name}\n\n";
                body += "Xin lưu ý rằng việc trừ tiền này đảm bảo bạn tiếp tục sử dụng các tính năng và dịch vụ của hệ thống chúng tôi.\n\n";
                body += "Nếu bạn có bất kỳ câu hỏi hoặc cần hỗ trợ thêm, xin vui lòng liên hệ với chúng tôi qua địa chỉ email hoặc số điện thoại dưới đây. Chúng tôi luôn sẵn sàng hỗ trợ bạn.\n\n";
                body += "Chân thành cảm ơn sự tin tưởng và ủng hộ của bạn đối với hệ thống của chúng tôi.\n\n";
                body += "Trân trọng,\n";
                body += "ParkZ\n";
                body += "Địa chỉ công ty: Lô E2a-7, Đường D1, Đ. D1, Long Thạnh Mỹ, Thành Phố Thủ Đức, Thành phố Hồ Chí Minh 700000\n";
                body += "Số điện thoại công ty: 0793808821\n";
                body += "Địa chỉ email công ty: parkz.thichthicodeteam@gmail.com\r\n";
                _emailService.SendMail(emailModel);
                Console.WriteLine("done email");
                //RecurringJob.AddOrUpdate<IServiceManagement>(x => x.ChargeMoneyFor1MonthUsingSystem(fee, bussinesId, newBill.BillId, user), Cron.MinuteInterval(6));
            }
            catch (Exception ex)
            {
                throw new Exception("Hangfire:" + ex.Message);
            }

        }

    }
}
