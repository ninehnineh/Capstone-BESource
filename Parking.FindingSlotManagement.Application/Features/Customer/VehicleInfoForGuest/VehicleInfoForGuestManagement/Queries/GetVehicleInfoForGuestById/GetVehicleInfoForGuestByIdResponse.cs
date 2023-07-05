using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Queries.GetVehicleInfoForGuestById
{
    public class GetVehicleInfoForGuestByIdResponse
    {
        public int VehicleInforId { get; set; }
        public string? LicensePlate { get; set; }
        public string? VehicleName { get; set; }
        public string? Color { get; set; }
    }
}
