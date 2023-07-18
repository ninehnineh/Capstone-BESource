using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Commands.DeclineParkingRequest
{
    public class DeclineParkingRequestCommand : IRequest<ServiceResponse<string>>
    {
        public int ApproveParkingId { get; set; }
        public string NoteForAdmin { get; set; }
    }
}
