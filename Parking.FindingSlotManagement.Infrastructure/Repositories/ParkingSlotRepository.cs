using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Exceptions;
using Parking.FindingSlotManagement.Application.Models.ParkingSlot;
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
    public class ParkingSlotRepository : GenericRepository<ParkingSlot>, IParkingSlotRepository
    {
        private readonly ParkZDbContext _dbContext;

        public ParkingSlotRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task DisableParkingSlotWhenAllTimeFree(int parkingSlotId)
        {
            try
            {
                var parkingSlot = await _dbContext.ParkingSlots.FindAsync(parkingSlotId);
                parkingSlot!.IsAvailable = false;

                await _dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at DisableParkingSlotWhenAllTimeFree: Message {ex.Message}");
            }
        }

        public async Task DisableParkingSlotWhenSomeTimeBooked(int parkingSlotId)
        {
            try
            {
                var parkingSlot = await _dbContext.ParkingSlots
                    .Include(x => x.TimeSlots)
                    .FirstOrDefaultAsync(x => x.ParkingSlotId == parkingSlotId);

                var bookedTimeSlots = parkingSlot.TimeSlots.Where(x => x.Status == TimeSlotStatus.Booked.ToString());
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Error at DisableParkingSlotWhenSomeTimeBooked: Message {ex.Message}");
            }
        }

        public async Task EnableParkingSlot(int parkingSlotId)
        {
            try
            {
                var disableParkingSlot = await _dbContext.ParkingSlots
                    .Include(x => x.TimeSlots)
                    .FirstOrDefaultAsync(x => x.ParkingSlotId == parkingSlotId);

                var timeSlot = disableParkingSlot.TimeSlots;
                foreach (var time in timeSlot)
                {
                    time.Status = TimeSlotStatus.Free.ToString();
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at EnableParkingSlot: Message {ex.Message}");
            }
        }

        public async Task<bool> GetParkingByParkingSlotId(int parkingSlotId)
        {
            try
            {
                var parkingSlot = await _dbContext.ParkingSlots
                    .Include(x => x.Floor)
                    .ThenInclude(x => x.Parking)
                    .FirstOrDefaultAsync(x => x.ParkingSlotId == parkingSlotId);

                if (parkingSlot == null)
                    throw new NotFoundException("parkingSlotId", parkingSlot);
                
                var result = parkingSlot.Floor.Parking.IsAvailable;
                
                return result.Value;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetFloorByParkingSlotId: Message {ex.Message}");
            }
        }

        public async Task<int> GetParkingSlotByParkingSlotId(int parkingSlotId)
        {
            try
            {
                var parkingId = await _dbContext.ParkingSlots
                    .Include(x => x.Floor)
                    .FirstOrDefaultAsync(x => x.ParkingSlotId == parkingSlotId);
                if (parkingId == null)
                {
                    return 0;
                }
                return (int)parkingId.Floor.ParkingId;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<ParkingSlot>> GetParkingSlotsByParkingId(int parkingId)
        {
            try
            {
                var parkingSlots = await _dbContext.ParkingSlots
                    .Include(x => x.Floor).ThenInclude(x => x!.Parking)
                    .Where(x => x.Floor!.ParkingId == parkingId)
                    .ToListAsync();

                return parkingSlots;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at EnableParkingSlot: Message {ex.Message}");
            }
        }

        public async Task<bool> isExists(ParkingSlotDTO parkingSlotDTO)
        {
            var slotNameExist = await _dbContext.ParkingSlots
                .AnyAsync(x =>
                    x.Name!.Trim().Equals(parkingSlotDTO.Name) &&
                    x.FloorId == parkingSlotDTO.FloorId
                );

            return slotNameExist;
        }

        public async Task<bool> IsNotAvailable(int parkingSlotId)
        {
            try
            {
                // var parking = await _dbContext.ParkingSlots
                //     .Include(x => x.Floor)
                //     .ThenInclude(x => x.Parking)
                //     .FirstOrDefaultAsync(x => x.Floor)


                // if (parking == null)
                //     throw new NotFoundException("parking", "parkingId");

                // parking.IsAvailable = false;

                return true;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at DisableParkingById: Message {ex.Message}");
            }
        }
        // public async Task<List<Domain.Entities.ParkingSlot>> GetParkingSlotsByParkingId(int parkingId)
        // {
        //     try
        //     {
        //         ArgumentNullException.ThrowIfNull(parkingId);

        //         var parkingSlots = await _dbContext.ParkingSlots
        //             .Include(x => x.Floor!);
        //     }
        //     catch (System.Exception ex)
        //     {
        //         throw new Exception($"Error at GetParkingSlotsByParkingId: Message {ex.Message}");
        //     }
        // }

        // public async Task DisableParkingSlotByDate(int parkingId, int parkingSlotId, )
        // {
        //     try
        //     {

        //     }
        //     catch (System.Exception ex)
        //     {

        //         throw new Exception($"Error at DisableParkingSlotByDate: Message {ex.Message}");
        //     }
        // }
    }
}
