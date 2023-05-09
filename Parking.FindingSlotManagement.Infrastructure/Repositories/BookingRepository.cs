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

        public async Task<Booking> GetBooking(BookingDTO bookingDTO)
        {
            var booking = await _dbContext.Bookings.FindAsync(
                bookingDTO.StartTime,
                bookingDTO.DateBook,
                bookingDTO.ParkingSlotId
            );
            if (booking == null) { return null; }
            return booking;
        }

        public async Task<Booking> GetBooking(int bookingId)
        {
            var booking = await _dbContext.Bookings
                .FirstOrDefaultAsync(x => x.BookingId == bookingId);
            if (booking == null) { return null; }
            return booking;
        }
    }
}
