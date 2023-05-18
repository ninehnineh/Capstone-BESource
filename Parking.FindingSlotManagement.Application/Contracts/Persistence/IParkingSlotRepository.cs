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
        //Task<bool> isExists(ParkingSlotDTO parkingSlotDTO);
    }
}
