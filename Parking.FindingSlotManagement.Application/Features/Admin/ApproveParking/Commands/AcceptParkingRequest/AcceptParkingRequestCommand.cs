using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Commands.AcceptParkingRequest
{
    public class AcceptParkingRequestCommand : IRequest<ServiceResponse<string>>
    {
        public int ApproveParkingId { get; set; }
        public string NoteForAdmin { get; set; }
    }
}
