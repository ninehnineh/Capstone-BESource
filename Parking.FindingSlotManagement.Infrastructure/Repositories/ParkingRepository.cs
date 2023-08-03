using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.DisableParkingSlotByDate.Model;
using Parking.FindingSlotManagement.Application.Models.Parking;
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
        private readonly ParkZDbContext dbContext;
        private readonly IMapper mapper;

        public ParkingRepository(ParkZDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<DisableSlotParking> GetParking(int parkingSlotId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(parkingSlotId);

                var parking = await dbContext.Parkings
                    .Include(x => x.BusinessProfile!.User)
                    .FirstOrDefaultAsync(p => p.Floors!
                        .Any(f => f.ParkingSlots!
                            .Any(ps => ps.ParkingSlotId == parkingSlotId)
                        )
                    );

                var response = new DisableSlotParking{ParkingId = parking!.ParkingId, ManagerId = parking.BusinessProfile.UserId!.Value};

                return response;
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Error at GetParking: Message {ex.Message}");
            }
        }

        public async Task<Domain.Entities.Parking> GetParkingById(int parkingId)
        {
            try
            {

                var parking = await dbContext.Parkings
                    .Include(x => x.Floors)!.ThenInclude(x => x.ParkingSlots)!.ThenInclude(x => x.TimeSlots)
                    .FirstOrDefaultAsync(x => x.ParkingId == parkingId);

                return parking;
            }
            catch (System.Exception ex)
            {
                
                throw new Exception($"Error at GetParkingById: Message {ex.Message}");
            }
        }
    }
}
