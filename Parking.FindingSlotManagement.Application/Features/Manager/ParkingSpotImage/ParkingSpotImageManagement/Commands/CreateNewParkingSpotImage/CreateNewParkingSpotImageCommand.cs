using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.CreateNewParkingSpotImage
{
    public class CreateNewParkingSpotImageCommand : IRequest<ServiceResponse<int>>
    {
        public string? ImgPath { get; set; }
        public int? ParkingId { get; set; }
    }
}
