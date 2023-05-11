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
        Task<Booking> GetBooking(BookingDTO bookingDTO);
        Task<Booking> GetBooking(int bookingId);
    }
}
