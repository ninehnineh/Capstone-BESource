using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Persistences;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ParkZDbContext dbContext;

        public UserRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Exists(int managerId)
        {
            var isExist = await dbContext.Users.FirstOrDefaultAsync(x => x.ManagerId == managerId);
            return isExist != null;
        }
    }
}