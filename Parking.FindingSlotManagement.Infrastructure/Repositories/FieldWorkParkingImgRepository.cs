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
    public class FieldWorkParkingImgRepository : GenericRepository<FieldWorkParkingImg>, IFieldWorkParkingImgRepository
    {
        private readonly ParkZDbContext _dbContext;

        public FieldWorkParkingImgRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeFieldWorkParkingImg(List<FieldWorkParkingImg> fwpi)
        {
            try
            {
                _dbContext.AddRangeAsync(fwpi);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
