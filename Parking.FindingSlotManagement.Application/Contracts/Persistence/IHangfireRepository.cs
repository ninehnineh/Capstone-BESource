using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface IHangfireRepository
    {
        Task<string> DeleteJob(int bookingId, string? methodName);
        Task<string> DeleteJob(int bookingId);
    }
}