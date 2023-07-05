using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.CreateNewApproveParking
{
    public class CreateNewApproveParkingCommand : IRequest<ServiceResponse<int>>
    {
        public int StaffId { get; set; }
        public int ParkingId { get; set; }
        public string Note { get; set; }
        public List<string> Images { get; set; }
    }
}
