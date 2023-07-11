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
    }
}
