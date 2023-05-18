using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.ParkingSlot;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class ParkingSlotRepository : GenericRepository<ParkingSlot>, IParkingSlotRepository
    {
        private readonly ParkZDbContext _dbContext;

        public ParkingSlotRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task<bool> isExists(ParkingSlotDTO parkingSlotDTO)
        //{
        //    var slotNameExist = await _dbContext.ParkingSlots
        //        .AnyAsync(x =>
        //            x.Name!.Trim().Equals(parkingSlotDTO.Name) &&
        //            x.FloorId == parkingSlotDTO.FloorId && 
        //            x.ParkingId == parkingSlotDTO.ParkingId
        //        );

        //    return slotNameExist;
        //}
    }
}
