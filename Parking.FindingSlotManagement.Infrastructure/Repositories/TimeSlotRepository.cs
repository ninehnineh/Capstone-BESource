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
    public class TimeSlotRepository : GenericRepository<TimeSlot>, ITimeSlotRepository
    {
        private readonly ParkZDbContext _dbContext;

        public TimeSlotRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> AddRangeTimeSlot(List<TimeSlot> lstTs)
        {
            try
            {
                _dbContext.TimeSlots.AddRangeAsync(lstTs);
                await _dbContext.SaveChangesAsync();
                return "Thành công";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
