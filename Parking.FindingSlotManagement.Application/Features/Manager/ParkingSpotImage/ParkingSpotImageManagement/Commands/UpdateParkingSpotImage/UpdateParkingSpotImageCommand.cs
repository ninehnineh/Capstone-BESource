using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.UpdateParkingSpotImage
{
    public class UpdateParkingSpotImageCommand : IRequest<ServiceResponse<string>>
    {
        public int ParkingSpotImageId { get; set; }
        public string? ImgPath { get; set; }
        public int? ParkingId { get; set; }
    }
}
