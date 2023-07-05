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
    }
}
