using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Keeper.Commands.DisableParkingSlotByDate.Model;

public class DisableParkingSlotByDateModel
{
    public DisableParkingSlotParking DisableParking { get; set; }
}

public class DisableParkingSlotParking
{
    public int ParkingId { get; set; }
    public bool? IsActive { get; set; }
    public virtual List<DisableParkingSlotFloor>? Floors { get; set; }
}

public class DisableParkingSlotFloor
{
    public int FloorId { get; set; }
    public string? FloorName { get; set; }
    public bool? IsActive { get; set; }
}
