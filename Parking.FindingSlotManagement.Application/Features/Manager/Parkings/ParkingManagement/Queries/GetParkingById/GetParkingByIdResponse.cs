using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetParkingById
{
    public class GetParkingByIdResponse
    {
        public ParkingEntity ParkingEntity { get; set; }
        public int NumberOfFloors { get; set; }
        public int NumberofParkingHasPrice { get; set; }
    }
}
