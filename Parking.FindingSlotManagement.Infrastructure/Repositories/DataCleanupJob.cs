using Hangfire;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class DataCleanupJob
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private static ParkZDbContext _context;

        public DataCleanupJob(IBackgroundJobClient backgroundJobClient, ParkZDbContext context)
        {
            _backgroundJobClient = backgroundJobClient;
            _context = context;
        }

        public static void DeleteOldData()
        {
            var oneWeekAgo = DateTime.UtcNow.AddHours(-7);
            var dataToDelete = _context.TimeSlots.Where(x => x.CreatedDate < oneWeekAgo);

            _context.TimeSlots.RemoveRange(dataToDelete);
            _context.SaveChanges();
        }

        public void ScheduleDataCleanup()
        {
            _backgroundJobClient.Enqueue(() => DeleteOldData());
        }
    }
}
