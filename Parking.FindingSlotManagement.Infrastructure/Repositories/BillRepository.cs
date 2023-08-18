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
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        private readonly ParkZDbContext _dbContext;

        public BillRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<decimal> GetRevenueOfBusinessByDateMethod(DateTime date)
        {
            try
            {
                var bill = await _dbContext.Bills.Where(x => x.Time.Date == date.Date && x.Status.Equals(BillStatus.Đã_Thanh_Toán.ToString()))
                                                .Select(x => x.Price)
                                                .ToListAsync();
                var totalRevenueOfTheDate = 0M;
                if (!bill.Any())
                {
                    return 0;
                }
                foreach (var item in bill)
                {
                    totalRevenueOfTheDate += (decimal)item;
                }
                return totalRevenueOfTheDate;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
