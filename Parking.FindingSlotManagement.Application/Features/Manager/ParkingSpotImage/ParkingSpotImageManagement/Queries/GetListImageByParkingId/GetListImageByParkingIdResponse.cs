using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Queries.GetListImageByParkingId
{
    public class GetListImageByParkingIdResponse
    {
        public int ParkingSpotImageId { get; set; }
        public string? ImgPath { get; set; }
    }
}
