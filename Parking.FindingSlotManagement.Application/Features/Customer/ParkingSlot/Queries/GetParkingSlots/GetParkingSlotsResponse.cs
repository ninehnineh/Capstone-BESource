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
    public int ParkingSlotId { get; set; }
    public string? Name { get; set; }
    public bool? IsAvailable { get; set; }
    public int? RowIndex { get; set; }
    public int? ColumnIndex { get; set; }
    public TrafficDto? Traffic { get; set; }
    public FloorDto? Floor { get; set; }
}

