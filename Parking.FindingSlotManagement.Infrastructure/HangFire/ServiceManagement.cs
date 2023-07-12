using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
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

        public ServiceManagement(ParkZDbContext context,
            ILogger<ServiceManagement> logger,
            IFireBaseMessageServices fireBaseMessageServices)
        {
            _context = context;
            _logger = logger;
            _fireBaseMessageServices = fireBaseMessageServices;
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
;
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
    }
}
