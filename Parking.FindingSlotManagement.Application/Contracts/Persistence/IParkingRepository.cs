using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.DisableParkingSlotByDate.Model;
using Parking.FindingSlotManagement.Application.Models.Parking;
using Parking.FindingSlotManagement.Domain.Entities;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface IParkingRepository : IGenericRepository<Domain.Entities.Parking>
    {
        Task<DisableSlotParking> GetParking(int parkingSlotId);
        Task<Domain.Entities.Parking> GetParkingById(int parkingId);
        Task EnableDisableParkingById(int parkingId, DateTime disableDate);
        Task<bool> GetDisableParking(int parkingId, DateTime disableDate);
        Task<int> GetManagerIdByParkingId(int parkingId);
        Task DisableParkingById(int parkingId);

    }
}