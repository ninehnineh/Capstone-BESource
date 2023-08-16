using Parking.FindingSlotManagement.Application.Models.ParkingSlot;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface IParkingSlotRepository : IGenericRepository<ParkingSlot>
    {
        Task<bool> isExists(ParkingSlotDTO parkingSlotDTO);
        Task DisableParkingSlotWhenAllTimeFree(int parkingSlotId);
        Task DisableParkingSlotWhenSomeTimeBooked(int parkingSlotId);
        Task EnableParkingSlot(int parkingSlotId);
        Task<int> GetParkingSlotByParkingSlotId(int parkingSlotId);
        Task<IEnumerable<ParkingSlot>> GetParkingSlotsByParkingId(int parkingId);
        Task<bool> IsNotAvailable(int parkingSlotId);
        Task<bool> GetParkingByParkingSlotId(int parkingSlotId);
    }
}
