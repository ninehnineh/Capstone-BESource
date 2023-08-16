using Parking.FindingSlotManagement.Application.Models.ParkingSlot;
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
        Task<string> AddRangeTimeSlot(List<TimeSlot> lstTs);
        Task DisableTimeSlot(int parkingSlotId);
        Task<List<DisableSlotResult>> GetBookedTimeSlotIncludeBookingDetails(int parkingSlotId);
        Task DisableTimeSlotByDisableDate(List<ParkingSlot> parkingSlotId, DateTime disableDate);
        Task<IEnumerable<List<TimeSlot>>> GetBookedTimeSlotsByDateNew(List<ParkingSlot> parkingSlotId, DateTime date);
        Task<IEnumerable<List<TimeSlot>>> GetBookedTimeSlotsByDateTime(List<ParkingSlot> parkingSlotId, DateTime date);
        // Task<List<List<TimeSlot>>> GetBookedTimeSlotByParkingSlotId(List<ParkingSlot> parkingSlots, DateTime disableDate);
        Task DisableTimeSlotByDisableDateTime(List<ParkingSlot> parkingSlots, DateTime disableDate);
        Task<bool> IsExist(DateTime disableDate);
        Task<int> GetBusyParkingSlotId(List<ParkingSlot> parkingSlotId);
    }
}
