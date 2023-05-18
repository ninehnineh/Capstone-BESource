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
    public class AccountRepository : GenericRepository<User>, IAccountRepository
    {
        private readonly ParkZDbContext _dbContext;

        public AccountRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Exists(string phoneNumber)
        {
            var exist = _dbContext.Users
                .Any(x => x.Phone!.Trim().Equals(phoneNumber.Trim()));

            return exist;
        }
    }
}
