using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetListParkingSlotByFloorId
{
    public class GetListParkingSlotByFloorIdResponse
    {
        public int ParkingSlotId { get; set; }
        public string? Name { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
        public bool? IsBackup { get; set; }
    }
}
