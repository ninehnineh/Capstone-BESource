using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class BookingDetailsRepository : GenericRepository<BookingDetails>, IBookingDetailsRepository
    {
        private readonly ParkZDbContext _dbContext;

        public BookingDetailsRepository(ParkZDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRange(List<BookingDetails> bookingDetails)
        {
            await _dbContext.AddRangeAsync(bookingDetails);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRange(List<BookingDetails> bookingDetails)
        {
            _dbContext.RemoveRange(bookingDetails);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingDetails>> GetParkingSlotIdByBookingDetail(int bookingId)
        {
            var booking = await _dbContext.BookingDetails
                                                .Include(x => x.TimeSlot)
                                                .Where(x => x.BookingId == bookingId).ToListAsync();
            if(!booking.Any())
            {
                return null;
            }
            return booking;
        }
    }
}
