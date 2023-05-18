using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Persistences;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class ParkingHasPriceRepository : GenericRepository<ParkingHasPrice>, IParkingHasPriceRepository
    {
        private readonly ParkZDbContext dbContext;

        public ParkingHasPriceRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Exists(int id)
        {
            var isExist = await dbContext.ParkingHasPrices
                .FindAsync(id);
            return isExist != null;
        }
    }
}