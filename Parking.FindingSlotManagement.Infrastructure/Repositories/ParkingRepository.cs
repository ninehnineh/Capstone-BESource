using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class ParkingRepository : GenericRepository<Domain.Entities.Parking>, IParkingRepository
    {
        public ParkingRepository(ParkZDbContext dbContext) : base(dbContext)
        {
        }
    }
}
