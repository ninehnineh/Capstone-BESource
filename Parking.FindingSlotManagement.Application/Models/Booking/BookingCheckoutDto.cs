using Parking.FindingSlotManagement.Application.Models.Transaction;
using Parking.FindingSlotManagement.Application.Models.VehicleInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Models.Booking
{
    public class BookingCheckoutDto
    {
        public int BookingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public DateTime DateBook { get; set; }
        public string? Status { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? QRImage { get; set; }
        public decimal UnPaidMoney { get; set; }
        public BookingVehicleInforDTO? VehicleInfor { get; set; }
        public IEnumerable<BookingTransactionDto>? Transactions { get; set; }
    }
}
