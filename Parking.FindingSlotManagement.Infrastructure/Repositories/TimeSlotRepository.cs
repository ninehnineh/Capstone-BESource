using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.ParkingSlot;
using Parking.FindingSlotManagement.Application.Models.TimeSlot;
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
    public class TimeSlotRepository : GenericRepository<TimeSlot>, ITimeSlotRepository
    {
        private readonly ParkZDbContext _dbContext;

        public TimeSlotRepository(ParkZDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<TimeSlot>> GetAllTimeSlotsBooking(DateTime startTimeBooking,
            DateTime endTimeBooking, int parkingSlotId)
        {
            try
            {
                var bookingTimeSlots = await _dbContext.TimeSlots
                    .Where(x => x.ParkingSlotId == parkingSlotId &&
                        x.StartTime >= startTimeBooking && x.EndTime <= endTimeBooking)
                    .ToListAsync();

                return bookingTimeSlots;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> AddRangeTimeSlot(List<TimeSlot> lstTs)
        {
            try
            {
                _dbContext.TimeSlots.AddRangeAsync(lstTs);
                await _dbContext.SaveChangesAsync();
                return "Thành công";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task DisableTimeSlot(int parkingSlotId)
        {
            try
            {
                var timeSlots = await _dbContext.TimeSlots.Where(x => x.ParkingSlotId == parkingSlotId).ToListAsync();
                timeSlots.ForEach(x => x.Status = TimeSlotStatus.Busy.ToString());

                await _dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at disable time slot repository dbcontext: {ex.Message}");
            }
        }

        public async Task<List<DisableSlotResult>> GetBookedTimeSlotIncludeBookingDetails(int parkingSlotId)
        {
            try
            {
                var list = new HashSet<DisableSlotResult>();
                var bookedTimeSlot = await _dbContext.TimeSlots
                    .Include(x => x.BookingDetails)!.ThenInclude(x => x.Booking!).ThenInclude(y => y.User!).ThenInclude(z => z.Wallet)
                    .Where(x => x.Status == TimeSlotStatus.Booked.ToString() &&
                                x.ParkingSlotId == parkingSlotId)
                    .ToListAsync();

                foreach (var timeSlot in bookedTimeSlot)
                {
                    list.UnionWith(timeSlot.BookingDetails!.Select(x => new DisableSlotResult
                    {
                        BookingId = x.BookingId!.Value,
                        Wallet = x.Booking!.User!.Wallet!,
                        User = x.Booking.User
                    }));
                }

                return list.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetBookedTimeSlot: Message {ex.Message}");
            }
        }

        public async Task DisableTimeSlotByDisableDate(List<ParkingSlot> parkingSlots, DateTime disableDate)
        {
            try
            {
                foreach (var slot in parkingSlots)
                {
                    var disableTimeSlots = await _dbContext.TimeSlots
                        .Where(x => x.ParkingSlotId == slot.ParkingSlotId && x.StartTime.Date == disableDate.Date)
                        .ToListAsync();

                    disableTimeSlots.ForEach(x => x.Status = TimeSlotStatus.Busy.ToString());
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Error at DisableTimeSlotByDisableDate: Message {ex.Message}");
            }
        }

        public async Task DisableTimeSlotByDisableDateTime(List<ParkingSlot> parkingSlots, DateTime disableDate)
        {
            try
            {
                foreach (var slot in parkingSlots)
                {
                    var disableTimeSlots = await _dbContext.TimeSlots
                        .Where(x => x.ParkingSlotId == slot.ParkingSlotId &&
                                    x.StartTime.Hour > disableDate.Hour &&
                                    x.StartTime.Date == disableDate.Date)
                        .ToListAsync();

                    disableTimeSlots.ForEach(x => x.Status = TimeSlotStatus.Busy.ToString());
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Error at DisableTimeSlotByDisableDate: Message {ex.Message}");
            }
        }

        public async Task<IEnumerable<List<TimeSlot>>> GetBookedTimeSlotsByDateNew(List<ParkingSlot> parkingSlotId, DateTime date)
        {
            try
            {
                List<List<TimeSlot>> result = new List<List<TimeSlot>>();

                foreach (var slot in parkingSlotId)
                {
                    var bookedTimeSlots = await _dbContext.TimeSlots
                        .Where(x => x.ParkingSlotId == slot.ParkingSlotId &&
                                    x.StartTime.Date == date &&
                                    x.Status.Equals(TimeSlotStatus.Booked.ToString()))
                        .ToListAsync();

                    if (bookedTimeSlots.Count != 0)
                        result.Add(bookedTimeSlots);
                }

                return result;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetBookedTimeSlotsByDate: Message {ex.Message}");
            }
        }


        public async Task<IEnumerable<List<TimeSlot>>> GetBookedTimeSlotsByDateTime(List<ParkingSlot> parkingSlotId, DateTime date)
        {
            try
            {
                List<List<TimeSlot>> result = new List<List<TimeSlot>>();

                foreach (var slot in parkingSlotId)
                {
                    var bookedTimeSlots = await _dbContext.TimeSlots
                       .Where(x => x.ParkingSlotId == slot.ParkingSlotId &&
                                   x.StartTime.Hour > date.Hour &&
                                   x.StartTime.Date == date.Date &&
                                   x.Status.Equals(TimeSlotStatus.Booked.ToString()))
                       .ToListAsync();

                    if (bookedTimeSlots.Count != 0)
                        result.Add(bookedTimeSlots);
                }

                return result;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetBookedTimeSlotsByDate: Message {ex.Message}");
            }
        }


        public async Task<int> GetBusyParkingSlotId(List<ParkingSlot> parkingSlotId)
        {
            try
            {
                var slotBaoTri = 0;
                foreach (var slot in parkingSlotId)
                {
                    var bookedTimeSlots = await _dbContext.TimeSlots
                       .Where(x => x.ParkingSlotId == slot.ParkingSlotId  &&
                                   x.Status.Equals(TimeSlotStatus.Busy.ToString()))
                       .ToListAsync();

                    if (bookedTimeSlots.Count != 0)
                        slotBaoTri = slot.ParkingSlotId;
                }
                return slotBaoTri;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetBookedTimeSlotsByDate: Message {ex.Message}");
            }
        }
        public async Task<bool> IsExist(DateTime disableDate)
        {
            try
            {
                var result = false;

                var timeslot = await _dbContext.TimeSlots
                    .FirstOrDefaultAsync(x => x.StartTime.Date == disableDate);

                if (timeslot != null)
                    result = true;

                return result;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at IsExist: Message {ex.Message}");
            }
        }

        // public async Task<List<List<TimeSlot>>> GetBookedTimeSlotByParkingSlotId(List<ParkingSlot> parkingSlots, DateTime disableDate)
        // {
        //     try
        //     {
        //         List<List<TimeSlot>> result = new List<List<TimeSlot>>();

        //         foreach (var parkingSlot in parkingSlots)
        //         {
        //             var timeSlots = await _dbContext.TimeSlots
        //                 .Include(x => x.BookingDetails)
        //                 .Where(x => x.ParkingSlotId == parkingSlot.ParkingSlotId &&
        //                             x.Status.Equals(TimeSlotStatus.Booked.ToString()) &&
        //                             x.CreatedDate.Date == disableDate)      
        //                 .ToListAsync();

        //             result.Add(timeSlots);
        //         }

        //         return result;
        //     }
        //     catch (System.Exception ex)
        //     {
        //         throw new Exception($"Error at GetBookedTimeSlotByParkingSlotId: Message {ex.Message}");
        //     }
        // }
    }

    // public class BookedUserWallet 
    // {
    //     public int WalletId { get; set; }
    //     public int Balance { get; set; }
    // }
}
