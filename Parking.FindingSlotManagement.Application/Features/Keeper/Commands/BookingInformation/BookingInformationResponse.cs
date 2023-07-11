using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Application.Models.Transaction;
using Parking.FindingSlotManagement.Application.Models.VehicleInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.BookingInformation;

public class BookingInformationResponse
{
    public int ParkingId { get; set; }
    public BookingCheckoutDto Booking { get; set; }

}

