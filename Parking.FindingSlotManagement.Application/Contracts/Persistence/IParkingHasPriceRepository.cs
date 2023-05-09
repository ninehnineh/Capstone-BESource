using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parking.FindingSlotManagement.Domain.Entities;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface IParkingHasPriceRepository : IGenericRepository<ParkingHasPrice>
    {
        Task<bool> Exists(int id);
    }
}