using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Staff.FieldWorkParkingImg.Commands.DeleteImagesById
{
    public class DeleteImagesByIdCommand : IRequest<ServiceResponse<string>>
    {
        public int FieldWorkParkingImgId { get; set; }
    }
}
