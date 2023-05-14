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
    public class PackagePriceRepository : GenericRepository<TimeLine>, IPackagePriceRepository
    {
        public PackagePriceRepository(ParkZDbContext dbContext) : base(dbContext)
        {
        }
    }
}
