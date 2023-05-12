using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.DeleteParkingSpotImage
{
    public class DeleteParkingSpotImageCommand : IRequest<ServiceResponse<string>>
    {
        public int ParkingSpotImageId { get; set; }
    }
}
