using Parking.FindingSlotManagement.Application.Models.TimeSlot;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface ITimeSlotRepository : IGenericRepository<TimeSlot>
    {
        Task<List<TimeSlot>> GetAllTimeSlotsBooking(DateTime startTimeBooking,
            DateTime endTimeBooking, int bookingSlotId);
    }
}
