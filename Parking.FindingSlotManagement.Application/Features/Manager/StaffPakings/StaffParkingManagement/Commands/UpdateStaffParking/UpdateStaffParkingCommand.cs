using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.StaffPakings.StaffParkingManagement.Commands.UpdateStaffParking
{
    public class UpdateStaffParkingCommand : IRequest<ServiceResponse<string>>
    {
        public int StaffParkingId { get; set; }
        public int? UserId { get; set; }
        public int? ParkingId { get; set; }
    }
}
