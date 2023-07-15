using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.FieldWorkParkingImg.Commands.CreateNewImage
{
    public class CreateNewImageCommand : IRequest<ServiceResponse<int>>
    {
        public int ApproveParkingId { get; set; }
        public List<string>? Images { get; set; }
    }
}
