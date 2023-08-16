using Firebase.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Math.EC;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly ParkZDbContext _dbContext;

        public BookingRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Booking> GetBooking(int bookingId)
        {
            var booking = await _dbContext.Bookings
                .Include(x => x.BookingDetails)!
                .ThenInclude(x => x.TimeSlot)
                .FirstOrDefaultAsync(x => x.BookingId == bookingId);

            if (booking == null) { return null!; }

            return booking;
        }

        public async Task<Booking> GetBookingIncludeParkingSlot(int bookingId)
        {
            var booking = await _dbContext.Bookings
                .Include(x => x.Transactions)
                .Include(x => x.User)
                    .ThenInclude(x => x.Wallet)
                .Include(x => x.BookingDetails)!
                    .ThenInclude(x => x.TimeSlot)
                        .ThenInclude(x => x.Parkingslot)
                .FirstOrDefaultAsync(x => x.BookingId == bookingId);

            if (booking == null) { return null!; }

            return booking;
        }
        public async Task<Booking> GetBookingIncludeUser(int bookingId)
        {
            var booking = await _dbContext.Bookings
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.BookingId == bookingId);

            if (booking == null) { return null!; }

            return booking;
        }

        public async Task<Booking> GetBookingIncludeTransaction(int bookingId)
        {
            var booking = await _dbContext.Bookings
                .Include(x => x.Transactions)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.BookingId == bookingId);

            if (booking == null) { return null!; }

            return booking;
        }

        public async Task<Booking> GetBookingIncludeTimeSlot(int bookingId)
        {
            var booking = await _dbContext.Bookings
                .Include(x => x.BookingDetails)!
                .ThenInclude(x => x.TimeSlot)
                .FirstOrDefaultAsync(x => x.BookingId == bookingId);

            if (booking == null) { return null!; }

            return booking;
        }

        public async Task<Booking> GetBookingInclude(int bookingId)
        {
            var booking = await _dbContext.Bookings
                .Include(x => x.Transactions)
                .Include(x => x.VehicleInfor)
                .Include(x => x.BookingDetails)!
                    .ThenInclude(x => x.TimeSlot)
                    .ThenInclude(x => x.Parkingslot)
                    .ThenInclude(x => x.Floor)
                .FirstOrDefaultAsync(x => x.BookingId == bookingId);

            if (booking == null) { return null!; }

            return booking;
        }
        public async Task<IEnumerable<Booking>> GetListBookingByManagerIdMethod(int businessId, int pageNo, int pageSize)
        {
            try
            {
                var booking = await _dbContext.Bookings
                                                .Include(x => x.User)
                                                .Include(x => x.VehicleInfor)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                    .ThenInclude(x => x.Parking)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.Parking.BusinessId == businessId).ToListAsync();
                if (!booking.Any())
                {
                    return null;
                }
                return booking.Skip(((int)pageNo - 1) * (int)pageSize)
                        .Take((int)pageSize);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Booking> GetListBookingByBookingIdMethod(int bookingId)
        {
            try
            {
                var booking = await _dbContext.Bookings
                                                .Include(x => x.User)
                                                .Include(x => x.VehicleInfor)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                    .ThenInclude(x => x.Parking)
                                                .FirstOrDefaultAsync(x => x.BookingId == bookingId);
                if (booking == null)
                {
                    return null;
                }
                return booking;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Booking>> GetListBookingFollowCalendarMethod(DateTime start, DateTime end)
        {
            try
            {
                var booking = await _dbContext.Bookings
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.DateBook.Date >= start.Date && x.DateBook.Date <= end.Date).ToListAsync();
                if (!booking.Any())
                {
                    return null;
                }
                return booking;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetListBookingDoneOrCancelByParkingIdMethod(int parkingId, string bookingStatus)
        {
            var booking = await _dbContext.Bookings
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId && x.Status == bookingStatus.ToString()).ToListAsync();
            if (!booking.Any())
            {
                return 0;
            }
            return booking.Count();
        }

        public async Task<decimal> GetRevenueByDateByParkingIdMethod(int parkingId, DateTime date)
        {
            var booking = await _dbContext.Bookings
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId && x.Status == BookingStatus.Done.ToString() && x.DateBook.Date == date.Date)
                                                .Select(x => x.TotalPrice)
                                                .ToListAsync();

            var totalRevenueOfTheDate = 0M;
            if (!booking.Any())
            {
                return 0;
            }
            foreach (var item in booking)
            {
                totalRevenueOfTheDate += (decimal)item;
            }
            return totalRevenueOfTheDate;
        }

        public async Task<int> GetTotalOrdersByParkingIdMethod(int parkingId)
        {
            var booking = await _dbContext.Bookings
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId).ToListAsync();
            if (!booking.Any())
            {
                return 0;
            }
            return booking.Count();
        }

        public async Task<decimal> GetRevenueByParkingIdMethod(int parkingId)
        {
            var booking = await _dbContext.Bookings
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId && x.Status == BookingStatus.Done.ToString())
                                                .Select(x => x.TotalPrice)
                                                .ToListAsync();

            var totalRevenueOfTheDate = 0M;
            if (!booking.Any())
            {
                return 0;
            }
            foreach (var item in booking)
            {
                totalRevenueOfTheDate += (decimal)item;
            }
            return totalRevenueOfTheDate;
        }

        public async Task<int> GetTotalNumberOfOrdersInCurrentDayByParkingIdMethod(int parkingId)
        {
            var booking = await _dbContext.Bookings
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId).ToListAsync();
            if (!booking.Any())
            {
                return 0;
            }
            List<Booking> filter = new();
            foreach (var item in booking)
            {
                if (item.DateBook.Date == DateTime.UtcNow.Date)
                {
                    filter.Add(item);
                }
            }
            return filter.Count();
        }
        public async Task<int> GetTotalWaitingOrdersByParkingIdMethod(int parkingId)
        {
            var booking = await _dbContext.Bookings
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId && x.Status == BookingStatus.Initial.ToString()).ToListAsync();
            if (!booking.Any())
            {
                return 0;
            }
            return booking.Count();
        }

        public async Task<Booking> GetBookingDetailsByBookingIdMethod(int bookingId)
        {
            try
            {
                var booking = await _dbContext.Bookings
                                                .Include(x => x.User)
                                                .Include(x => x.Transactions)
                                                .Include(x => x.VehicleInfor)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                    .ThenInclude(x => x.Parking)
                                                .FirstOrDefaultAsync(x => x.BookingId == bookingId);
                if (booking == null)
                {
                    return null;
                }
                return booking;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Booking>> SearchRequestBookingMethod(int parkingId, string searchString)
        {
            int number;
            if (int.TryParse(searchString, out number))
            {
                var booking0 = await _dbContext.Bookings
                                                .Include(x => x.User)
                                                .Include(x => x.VehicleInfor)
                                                    .ThenInclude(x => x.Traffic)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId &&
                                                x.BookingId == int.Parse(searchString)
                                                ).ToListAsync();
                if (!booking0.Any())
                {
                    return null;
                }
                return booking0;

            }
            else
            {
                var booking = await _dbContext.Bookings
                                                .Include(x => x.User)
                                                .Include(x => x.VehicleInfor)
                                                    .ThenInclude(x => x.Traffic)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId &&
                                                x.User.Name.Contains(searchString.ToString()) && x.EndTime.Value.Date >= DateTime.UtcNow.Date
                                                ).ToListAsync();
                if (!booking.Any())
                {
                    var booking2 = await _dbContext.Bookings
                                                    .Include(x => x.User)
                                                    .Include(x => x.VehicleInfor)
                                                        .ThenInclude(x => x.Traffic)
                                                    .Include(x => x.BookingDetails)
                                                        .ThenInclude(x => x.TimeSlot)
                                                        .ThenInclude(x => x.Parkingslot)
                                                        .ThenInclude(x => x.Floor)
                                                    .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId &&
                                                    x.VehicleInfor.LicensePlate.Contains(searchString.ToString()) && x.EndTime.Value.Date >= DateTime.UtcNow.Date
                                                    ).ToListAsync();
                    if (!booking2.Any())
                    {
                        return null;
                    }
                    return booking2;
                }
                return booking;
            }
            return null;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingByParkingIdMethod(int parkingId, int pageNo, int pageSize)
        {
            try
            {
                var booking = await _dbContext.Bookings
                                                .Include(x => x.User)
                                                .Include(x => x.VehicleInfor)
                                                    .ThenInclude(x => x.Traffic)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId).ToListAsync();
                if (!booking.Any())
                {
                    return null;
                }
                return booking.Skip(((int)pageNo - 1) * (int)pageSize)
                        .Take((int)pageSize);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<Booking>> FilterBookingForKeeperMethod(int parkingId, DateTime? date, string? status, int pageNo, int pageSize)
        {
            try
            {
                List<Booking> booking = new();
                if (date != null && status != null)
                {
                    booking = await _dbContext.Bookings
                                                .Include(x => x.User)
                                                .Include(x => x.VehicleInfor)
                                                    .ThenInclude(x => x.Traffic)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId && x.DateBook.Date == date.Value.Date && x.Status.Equals(status.ToString())).ToListAsync();
                    if (!booking.Any())
                    {
                        return null;
                    }
                    return booking.Skip(((int)pageNo - 1) * (int)pageSize)
                            .Take((int)pageSize);
                }
                else if (date != null)
                {
                    booking = await _dbContext.Bookings
                                                .Include(x => x.User)
                                                .Include(x => x.VehicleInfor)
                                                    .ThenInclude(x => x.Traffic)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId && x.DateBook.Date == date.Value.Date).ToListAsync();
                    if (!booking.Any())
                    {
                        return null;
                    }
                    return booking.Skip(((int)pageNo - 1) * (int)pageSize)
                            .Take((int)pageSize);
                }
                else if (status != null)
                {
                    booking = await _dbContext.Bookings
                                                .Include(x => x.User)
                                                .Include(x => x.VehicleInfor)
                                                    .ThenInclude(x => x.Traffic)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId && x.Status.Equals(status.ToString())).ToListAsync();
                    if (!booking.Any())
                    {
                        return null;
                    }
                    return booking.Skip(((int)pageNo - 1) * (int)pageSize)
                            .Take((int)pageSize);
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Booking>> GetUpcommingBookingByUserIdMethod(int userId)
        {
            var booking = await _dbContext.Bookings
                                                 .Include(x => x.VehicleInfor)
                                                    .ThenInclude(x => x.Traffic)
                                                 .Include(x => x.BookingDetails)
                                                     .ThenInclude(x => x.TimeSlot)
                                                     .ThenInclude(x => x.Parkingslot)
                                                     .ThenInclude(x => x.Floor)
                                                     .ThenInclude(x => x.Parking)
                                                 .Where(x => x.UserId == userId && x.Status.Equals(BookingStatus.Initial.ToString()) ||
                                                 x.UserId == userId && x.Status.Equals(BookingStatus.Success.ToString()) ||
                                                 x.UserId == userId && x.Status.Equals(BookingStatus.Check_In.ToString()) ||
                                                 x.UserId == userId && x.Status.Equals(BookingStatus.Check_Out.ToString()) ||
                                                 x.UserId == userId && x.Status.Equals(BookingStatus.OverTime.ToString())).ToListAsync();
            if (!booking.Any())
            {
                return null;
            }
            return booking;
        }

        public async Task<IEnumerable<Booking>> GetCustomerActivitiesByUserIdMethod(int userId)
        {
            var booking = await _dbContext.Bookings
                                                 .Include(x => x.VehicleInfor)
                                                    .ThenInclude(x => x.Traffic)
                                                 .Include(x => x.BookingDetails)
                                                     .ThenInclude(x => x.TimeSlot)
                                                     .ThenInclude(x => x.Parkingslot)
                                                     .ThenInclude(x => x.Floor)
                                                     .ThenInclude(x => x.Parking)
                                                 .Where(x => x.UserId == userId && x.Status.Equals(BookingStatus.Done.ToString()) ||
                                                 x.UserId == userId && x.Status.Equals(BookingStatus.Cancel.ToString())).ToListAsync();
            if (!booking.Any())
            {
                return null;
            }
            return booking;
        }

        public async Task<Booking> GetBookingToFindAvailableSlotMethod(int bookingId)
        {
            var booking = await _dbContext.Bookings
                                                 .Include(x => x.BookingDetails)
                                                     .ThenInclude(x => x.TimeSlot)
                                                     .ThenInclude(x => x.Parkingslot)
                                                 .FirstOrDefaultAsync(x => x.BookingId == bookingId);
            if (booking == null)
            {
                return null;
            }
            return booking;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingForAdminMethod(int pageNo, int pageSize)
        {
            var booking = await _dbContext.Bookings
                                     .Include(x => x.User)
                                     .Include(x => x.BookingDetails)
                                         .ThenInclude(x => x.TimeSlot)
                                         .ThenInclude(x => x.Parkingslot)
                                         .ThenInclude(x => x.Floor)
                                         .ThenInclude(x => x.Parking)
                                     .OrderByDescending(x => x.BookingId).ToListAsync();
            if (!booking.Any())
            {
                return null;
            }
            return booking.Skip(((int)pageNo - 1) * (int)pageSize)
                            .Take((int)pageSize);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingByParkingIdVer2Method(int parkingId, int pageNo, int pageSize)
        {
            try
            {
                var booking = await _dbContext.Bookings
                                                .Include(x => x.User)
                                                .Include(x => x.VehicleInfor)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                    .ThenInclude(x => x.Parking)
                                                .Where(x => x.BookingDetails.FirstOrDefault().TimeSlot.Parkingslot.Floor.ParkingId == parkingId).OrderByDescending(x => x.BookingId).ToListAsync();
                if (!booking.Any())
                {
                    return null;
                }
                return booking.Skip(((int)pageNo - 1) * (int)pageSize)
                        .Take((int)pageSize);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Booking>> GetAllBookingWithDuplicateVehicle(int userId, string licensePlate)
        {
            var booking = await _dbContext.Bookings
                                                .Include(x => x.VehicleInfor)
                                                .Include(x => x.BookingDetails)
                                                    .ThenInclude(x => x.TimeSlot)
                                                    .ThenInclude(x => x.Parkingslot)
                                                    .ThenInclude(x => x.Floor)
                                                    .ThenInclude(x => x.Parking)
                                                .Where(x => x.UserId == userId && x.VehicleInfor.LicensePlate.Equals(licensePlate.ToString())
                                                && !x.Status.Equals(BookingStatus.Done.ToString()) && !x.Status.Equals(BookingStatus.Cancel.ToString())).ToListAsync();
            if (!booking.Any())
            {
                return null;
            }
            return booking;
        }

        public async Task<bool> GetBookingStatusByParkingSlotId(int parkingSlotId)
        {
            try
            {
                bool isCheckIn;

                var bookingStatus = await _dbContext.Bookings
                    .Include(x => x.BookingDetails)!
                    .ThenInclude(x => x.TimeSlot)!
                    .ThenInclude(x => x.Parkingslot)
                    .FirstOrDefaultAsync(x => x.BookingDetails!.Any(x => x.TimeSlot!.Parkingslot!.ParkingSlotId! == parkingSlotId) &&
                                                x.Status.Equals(BookingStatus.Check_In.ToString()));

                if (bookingStatus == null)
                {
                    isCheckIn = false;
                }
                else
                {
                    isCheckIn = true;
                }
                // var isCheckIn = bookingStatus.Status.Equals(BookingStatus.Check_In.ToString()) ? true : false;

                return isCheckIn;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetBookingStatusByParkingSlotId: Message {ex.Message}");
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsByBookingDetailId(List<BookingDetails> bookingDetails)
        {
            try
            {
                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetBookingsByBookingDetailId: Message {ex.Message}");
            }
        }

        public async Task CancelBookedBookingWhenDisableParking(List<BookingDetails> bookings)
        {
            try
            {
                foreach (var booking in bookings)
                {
                    var book = await _dbContext.Bookings
                        .FindAsync(booking.BookingId);
                    book.Status = BookingStatus.Cancel.ToString();
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at CancelBookedBookingWhenDisableParking: Message {ex.Message}");
            }
        }

        
        public async Task<List<Domain.Entities.User>> GetUsersByBookingId(List<BookingDetails> bookingDetails)
        {
            try
            {
                List<Domain.Entities.User> result = new List<Domain.Entities.User>();

                foreach (var bookingDetail in bookingDetails)
                {
                    var booking = await _dbContext.Bookings
                        .Include(x => x.User)
                        .FirstOrDefaultAsync(x => x.BookingId == bookingDetail.BookingId);

                    if (booking != null)
                        result.Add(booking.User);                        
                }

                return result;
            }
            catch (System.Exception ex)
            {
                
                throw new Exception($"Error at GetUsersByBookingId: Message {ex.Message}");
            }
        }
    }
}
