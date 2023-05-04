using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.StaffPakings.StaffParkingManagement.Commands.CreateNewStaffParking
{
    public class CreateNewStaffParkingCommand : IRequest<ServiceResponse<int>>
    {
        public int? UserId { get; set; }
        public int? ParkingId { get; set; }
    }
}
