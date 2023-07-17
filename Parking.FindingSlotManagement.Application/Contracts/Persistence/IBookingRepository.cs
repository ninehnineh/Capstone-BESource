using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Contracts.Persistence
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<Booking> GetBooking(int bookingId);
        Task<Booking> GetBookingIncludeTransaction(int bookingId);
        Task<Booking> GetBookingIncludeUser(int bookingId);
        Task<Booking> GetBookingIncludeParkingSlot (int bookingId);
        Task<Booking> GetBookingIncludeTimeSlot(int bookingId);
        Task<Booking> GetBookingInclude(int bookingId);
        Task<IEnumerable<Booking>> GetListBookingByManagerIdMethod(int businessId, int pageNo, int pageSize);
        Task<IEnumerable<Booking>> GetListBookingFollowCalendarMethod(DateTime start, DateTime end);
        Task<Booking> GetListBookingByBookingIdMethod(int bookingId);
        Task<int> GetListBookingDoneOrCancelByParkingIdMethod(int parkingId, string bookingStatus);
        Task<decimal> GetRevenueByDateByParkingIdMethod(int parkingId, DateTime date);
        Task<int> GetTotalOrdersByParkingIdMethod(int parkingId);
        Task<decimal> GetRevenueByParkingIdMethod(int parkingId);
        Task<int> GetTotalNumberOfOrdersInCurrentDayByParkingIdMethod(int parkingId);
        Task<int> GetTotalWaitingOrdersByParkingIdMethod(int parkingId);
        Task<Booking> GetBookingDetailsByBookingIdMethod(int bookingId);
        Task<IEnumerable<Booking>> SearchRequestBookingMethod(int parkingId, string searchString);
        Task<IEnumerable<Booking>> GetAllBookingByParkingIdMethod(int parkingId, int pageNo, int pageSize);
    }
}
