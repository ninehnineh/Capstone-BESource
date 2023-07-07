using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
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
                .Include(x => x.User)
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
                if(!booking.Any())
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
    }
}
