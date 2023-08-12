using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parking.FindingSlotManagement.Application.Models.Parking;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface IHangfireRepository
    {
        Task<string> DeleteJob(int bookingId, string? methodName);
        Task<string> DeleteJob(int bookingId);
        Task<IEnumerable<Job>> GetHistoryDisableParking(int parkingId);
        Task<string> DeleteScheduledJob(int parkingId, DateTime disableDate);
    }
}