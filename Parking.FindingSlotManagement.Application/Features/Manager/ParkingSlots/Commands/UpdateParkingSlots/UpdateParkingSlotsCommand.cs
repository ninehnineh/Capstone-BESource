using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.UpdateParkingSlots
{
    public class UpdateParkingSlotsCommand : IRequest<ServiceResponse<string>>
    {
        public int ParkingSlotId { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
    }
}
