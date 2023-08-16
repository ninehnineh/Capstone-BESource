using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetParkingInformationTab
{
    public class GetParkingInformationTabResponse
    {
        public int ParkingId { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string ApproveParkingStatus { get; set; }
        public float? Stars { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public bool? IsFull { get; set; }
        public int TotalFloor { get; set; }
        public int? CarSpot { get; set; }
        public int? SlotHasBooked { get; set; }
        public List<ListParkingPrices> ListParkingPrices { get; set; }
        public List<string> Images { get; set; }
    }
    public class ListParkingPrices
    {
        public int ParkingPriceId { get; set; }
        public string? ParkingPriceName { get; set; }
    }
}
