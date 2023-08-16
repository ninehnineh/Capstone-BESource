using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Domain.Enum;
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
            try
            {
                await _dbContext.AddRangeAsync(bookingDetails);
                await _dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception("Add Range Booking Details" + ex.Message);
            }

        }

        public async Task DeleteRange(List<BookingDetails> bookingDetails)
        {
            _dbContext.RemoveRange(bookingDetails);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingDetails>> GetBookingDetailsByTimeSlotId(List<TimeSlot> timeSlots)
        {
            try
            {
                
                List<BookingDetails> bookingDetails = new List<BookingDetails>();

                foreach (var timeSlot in timeSlots)
                {
                    var bookedBookingDetails = await _dbContext.BookingDetails
                        .Include(x => x.Booking)
                        .FirstOrDefaultAsync(x => x.TimeSlotId == timeSlot!.TimeSlotId && x.Booking.Status.Equals(BookingStatus.Success.ToString()));

                    bookingDetails.Add(bookedBookingDetails!);
                };                

                return bookingDetails;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetBookingDetailsByTimeSlotId: Message {ex.Message}");
            }
        }

        public async Task<IEnumerable<BookingDetails>> GetParkingSlotIdByBookingDetail(int bookingId)
        {
            var booking = await _dbContext.BookingDetails
                                                .Include(x => x.TimeSlot)
                                                .Where(x => x.BookingId == bookingId).ToListAsync();
            if (!booking.Any())
            {
                return null;
            }
            return booking;
        }
    }
}
