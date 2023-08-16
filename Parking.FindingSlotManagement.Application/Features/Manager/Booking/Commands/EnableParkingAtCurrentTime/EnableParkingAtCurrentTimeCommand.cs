using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Booking.Commands.EnableParkingAtCurrentTime
{
    public class EnableParkingAtCurrentTimeCommand : IRequest<ServiceResponse<string>>
    {
        public int ParkingId { get; set; }
        public DateTime DisableDate { get; set; }
    }
}