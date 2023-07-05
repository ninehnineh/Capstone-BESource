using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.TimeSlot;
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


        public async Task<List<TimeSlot>> GetAllTimeSlotsBooking(DateTime startTimeBooking,
            DateTime endTimeBooking, int parkingSlotId)
        {
            try
            {
                var bookingTimeSlots = await _dbContext.TimeSlots
                    .Where(x => x.ParkingSlotId == parkingSlotId &&
                        x.StartTime >= startTimeBooking && x.EndTime <= endTimeBooking)
                    .ToListAsync();
                
                return bookingTimeSlots;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
