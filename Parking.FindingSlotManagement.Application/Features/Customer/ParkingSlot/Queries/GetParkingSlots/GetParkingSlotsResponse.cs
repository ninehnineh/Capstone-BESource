using Parking.FindingSlotManagement.Application.Models.Floor;
using Parking.FindingSlotManagement.Application.Models.Traffic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetParkingSlots;

public class GetParkingSlotsResponse
{
    public IEnumerable<ParkingSlotsDto> ParkingSlots { get; set; }
    public CarSpot Car { get; set; }
    public MoToSpot MoTo { get; set; }
}

public class MoToSpot
{
    public int AvailableMoToSlots { get; set; }
    public int TotalNumberMoToSlots { get; set; }
}

public class CarSpot
{
    public int AvailableCarSlots { get; set; }
    public int TotalNumberCarSlots { get; set; }

}

public class ParkingSlotsDto
{
    public int ParkingSlotId { get; set; }
    public string? Name { get; set; }
    public bool? IsAvailable { get; set; }
    public int? RowIndex { get; set; }
    public int? ColumnIndex { get; set; }
    public TrafficDto? Traffic { get; set; }
    public FloorDto? Floor { get; set; }
}