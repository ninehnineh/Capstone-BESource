using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.UpdateApproveParking
{
    public class UpdateApproveParkingCommand : IRequest<ServiceResponse<int>>
    {
        public int ApproveParkingId { get; set; }
        public string Note { get; set; }
    }
}
